using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderItemRepository(AppDbContext _context)
    : GenericRepository<TenderItem>(_context), ITenderItemRepository
    {
        public override async Task<TenderItem?> GetByIdAsync(int id)
        {
            return await _context.TenderItems
                .Include(ti => ti.Medicine)
                .FirstOrDefaultAsync(ti => ti.Id == id);

        }

        public async Task<IEnumerable<TenderItem>> GetItemsByTenderIdAsync(int tenderId)
        {
            return await _context.TenderItems
                .Where(ti => ti.TenderId == tenderId)
                .Include(ti => ti.Medicine)
                .ToListAsync();
        }

    }
}
