using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class TenderProposalRepository(AppDbContext _context, ILogger<TenderProposalRepository> _logger) : ITenderProposalRepository
    {
        public async Task<TenderProposal?> GetByIdAsync(int id)
        {
            return await _context.TenderProposals
                .Include(p => p.Tender)
                .Include(p => p.Distributor)
                .Include(p => p.Items)
                .ThenInclude(i => i.Medicine)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<TenderProposal>> GetAllProposalsForTenderAsync(int tenderId)
        {
            return await _context.TenderProposals
                .Include(p => p.Distributor)
                .Include(p => p.Items)
                .ThenInclude(i => i.Medicine)
                .Where(p => p.TenderId == tenderId)
                .ToListAsync();
        }

        public async Task<List<TenderProposal>> GetProposalsByDistributorAsync(int distributorId)
        {
            return await _context.TenderProposals
                .Include(p => p.Tender)
                .Include(p => p.Items)
                .Where(p => p.DistributorId == distributorId)
                .ToListAsync();
        }

        public async Task<List<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status)
        {
            return await _context.TenderProposals
                .Include(p => p.Tender)
                .Include(p => p.Distributor)
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<bool> SubmitProposalAsync(TenderProposal proposal)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tender = await _context.Tenders.FindAsync(proposal.TenderId);
                if (tender == null) return false;
                if (tender.Status != TenderStatus.Published) return false;
                if (DateTime.UtcNow > tender.DeadlineDate) return false;

                proposal.SubmissionDate = DateTime.UtcNow;
                proposal.Status = ProposalStatus.Submitted;

                proposal.TotalPrice = proposal.Items.Sum(i => i.UnitPrice * i.Quantity);

                _context.TenderProposals.Add(proposal);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error submitting proposal");
                return false;
            }
        }

        public async Task<bool> UpdateProposalStatusAsync(int proposalId, ProposalStatus status)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var proposal = await _context.TenderProposals
                    .Include(p => p.Tender)
                    .FirstOrDefaultAsync(p => p.Id == proposalId);

                if (proposal == null) return false;
                if (proposal.Tender.Status != TenderStatus.Closed) return false;

                proposal.Status = status;

                if (status == ProposalStatus.Accepted)
                {
                    proposal.Tender.Status = TenderStatus.Awarded;

                    // Reject all other proposals
                    var otherProposals = await _context.TenderProposals
                        .Where(p => p.TenderId == proposal.TenderId && p.Id != proposalId)
                        .ToListAsync();

                    foreach (var otherProposal in otherProposals)
                    {
                        otherProposal.Status = ProposalStatus.Rejected;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating proposal status");
                return false;
            }
        }

        public async Task<bool> DeleteProposalAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var proposal = await _context.TenderProposals
                    .Include(p => p.Tender)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (proposal == null ||
                    proposal.Status != ProposalStatus.Submitted ||
                    proposal.Tender.Status != TenderStatus.Published)
                {
                    return false;
                }

                _context.TenderProposals.Remove(proposal);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting proposal");
                return false;
            }
        }

        public async Task<bool> AddProposalItemAsync(TenderProposalItem item)
        {
            try
            {
                var proposal = await _context.TenderProposals
                    .Include(p => p.Tender)
                    .FirstOrDefaultAsync(p => p.Id == item.TenderProposalId);

                if (proposal == null ||
                    proposal.Status != ProposalStatus.Submitted ||
                    proposal.Tender.Status != TenderStatus.Published)
                {
                    return false;
                }

                _context.TenderProposalItems.Add(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding proposal item");
                return false;
            }
        }

        public async Task<bool> UpdateProposalItemAsync(TenderProposalItem item)
        {
            try
            {
                var existingItem = await _context.TenderProposalItems
                    .FirstOrDefaultAsync(i => i.Id == item.Id && i.TenderProposalId == item.TenderProposalId);

                if (existingItem == null)
                {
                    return false;
                }

                existingItem.UnitPrice = item.UnitPrice;
                existingItem.Quantity = item.Quantity;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating proposal item");
                return false;
            }
        }

        public async Task<bool> RemoveProposalItemAsync(int itemId)
        {
            try
            {
                var item = await _context.TenderProposalItems
                    .FirstOrDefaultAsync(i => i.Id == itemId);

                if (item == null)
                {
                    return false;
                }

                _context.TenderProposalItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing proposal item");
                return false;
            }
        }

        public async Task<TenderProposal?> GetWinningProposalForTenderAsync(int tenderId)
        {
            return await _context.TenderProposals
                .Include(p => p.Distributor)
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.TenderId == tenderId && p.Status == ProposalStatus.Accepted);
        }

    }
}
