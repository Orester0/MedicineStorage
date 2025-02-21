using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderProposalRepository(AppDbContext _context)
    : GenericRepository<TenderProposal>(_context), ITenderProposalRepository
    {
        public override async Task<TenderProposal?> GetByIdAsync(int id)
        {
            return await _context.TenderProposals
                .Include(tp => tp.CreatedByUser)
                .Include(tp => tp.Items)
                .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public override async Task<List<TenderProposal>> GetAllAsync()
        {
            return await _context.TenderProposals
                .Include(tp => tp.CreatedByUser)
                .Include(tp => tp.Items)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId)
        {
            return await _context.TenderProposals
                .Where(tp => tp.TenderId == tenderId)
                .Include(tp => tp.CreatedByUser)
                .Include(tp => tp.Items)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsCreatedByUserIdAsync(int userId)
        {
            return await _context.TenderProposals
                .Where(tp => tp.CreatedByUserId == userId)
                .Include(tp => tp.CreatedByUser)
                .Include(tp => tp.Items)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status)
        {
            return await _context.TenderProposals
                .Where(tp => tp.Status == status)
                .Include(tp => tp.CreatedByUser)
                .Include(tp => tp.Items)
                .ToListAsync();
        }

    }
}
