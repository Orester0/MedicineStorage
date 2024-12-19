using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderProposalRepository(AppDbContext _context) : ITenderProposalRepository
    {
        public async Task<TenderProposal> GetProposalByIdAsync(int id)
        {
            return await _context.TenderProposals
                .Include(tp => tp.Tender)
                .Include(tp => tp.CreatedByUser)
                .Include(tp => tp.Items)
                .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task<IEnumerable<TenderProposal>> GetAllAsync()
        {
            return await _context.TenderProposals
                .Include(tp => tp.Tender)
                .Include(tp => tp.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId)
        {
            return await _context.TenderProposals
                .Where(tp => tp.TenderId == tenderId)
                .Include(tp => tp.Tender)
                .Include(tp => tp.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsCreatedByUserIdAsync(int userId)
        {
            return await _context.TenderProposals
                .Where(tp => tp.CreatedByUserId == userId)
                .Include(tp => tp.Tender)
                .Include(tp => tp.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status)
        {
            return await _context.TenderProposals
                .Where(tp => tp.Status == status)
                .Include(tp => tp.Tender)
                .Include(tp => tp.CreatedByUser)
                .ToListAsync();
        }



        public async Task<TenderProposal> CreateTenderProposalAsync(TenderProposal tenderProposal)
        {
            await _context.TenderProposals.AddAsync(tenderProposal);
            await _context.SaveChangesAsync();
            return tenderProposal;
        }

        public async Task UpdateTenderProposalAsync(TenderProposal tenderProposal)
        {
            _context.TenderProposals.Update(tenderProposal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTenderProposalAsync(int id)
        {
            var tenderProposal = await _context.TenderProposals.FindAsync(id);
            if (tenderProposal != null)
            {
                _context.TenderProposals.Remove(tenderProposal);
                await _context.SaveChangesAsync();
            }
        }

    }
}
