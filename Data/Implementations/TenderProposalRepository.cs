using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderProposalRepository(AppDbContext _context) : ITenderProposalRepository
    {
        public async Task<TenderProposal> GetByIdAsync(int id)
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

        public async Task<TenderProposal> AddAsync(TenderProposal tenderProposal)
        {
            await _context.TenderProposals.AddAsync(tenderProposal);
            await _context.SaveChangesAsync();
            return tenderProposal;
        }

        public async Task UpdateAsync(TenderProposal tenderProposal)
        {
            _context.TenderProposals.Update(tenderProposal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tenderProposal = await _context.TenderProposals.FindAsync(id);
            if (tenderProposal != null)
            {
                _context.TenderProposals.Remove(tenderProposal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId)
        {
            return await _context.TenderProposals
                .Where(tp => tp.TenderId == tenderId)
                .Include(tp => tp.Tender)
                .Include(tp => tp.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsByDistributorAsync(int distributorId)
        {
            return await _context.TenderProposals
                .Where(tp => tp.CreatedByUserId == distributorId)
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
    }
}
