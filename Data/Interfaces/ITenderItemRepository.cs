using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderItemRepository
    {
        public Task<TenderItem?> GetByIdAsync(int id);

        public Task<IEnumerable<TenderItem>> GetAllAsync();

        public Task<IEnumerable<TenderItem>> GetByTenderIdAsync(int tenderId);

        public Task<TenderItem> AddAsync(TenderItem tenderItem);

        public Task UpdateAsync(TenderItem tenderItem);

        public Task DeleteAsync(TenderItem tenderItem);
    }
}
