using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderItemRepository(AppDbContext _context) : ITenderItemRepository
    {
        public async Task<TenderItem?> GetByIdAsync(int id)
        {
            return await _context.TenderItems
                .Include(ti => ti.Medicine)
                .FirstOrDefaultAsync(ti => ti.Id == id);
                
        }

        public async Task<IEnumerable<TenderItem>> GetAllAsync()
        {
            return await _context.TenderItems.ToListAsync();
        }

        public async Task<IEnumerable<TenderItem>> GetItemsByTenderIdAsync(int tenderId)
        {
            return await _context.TenderItems
                .Where(ti => ti.TenderId == tenderId)
                .Include(ti => ti.Medicine)
                .ToListAsync();
        }

        public async Task<TenderItem> CreateTenderItemAsync(TenderItem tenderItem)
        {
            await _context.TenderItems.AddAsync(tenderItem);
            return tenderItem;
        }

        public void UpdateTenderItem(TenderItem tenderItem)
        {
            _context.TenderItems.Update(tenderItem);
        }

        public void DeleteTenderItem(TenderItem tenderItem)
        {
            _context.TenderItems.Remove(tenderItem);
        }
    }
}
