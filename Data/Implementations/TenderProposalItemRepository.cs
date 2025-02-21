using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderProposalItemRepository(AppDbContext _context)
    : GenericRepository<TenderProposalItem>(_context), ITenderProposalItemRepository
    {
        public override async Task<TenderProposalItem?> GetByIdAsync(int id)
        {
            return await _context.TenderProposalItems
                .Include(tpi => tpi.Medicine)
                .FirstOrDefaultAsync(tpi => tpi.Id == id);
        }

        public async Task<IEnumerable<TenderProposalItem>> GetItemsByProposalIdAsync(int proposalId)
        {
            return await _context.TenderProposalItems
                .Where(tpi => tpi.TenderProposalId == proposalId)
                .Include(tpi => tpi.Medicine)
                .ToListAsync();
        }


    }
}
