using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderItemRepository
    {
        public Task<TenderItem?> GetByIdAsync(int id);

        public Task<IEnumerable<TenderItem>> GetAllAsync();

        public Task<IEnumerable<TenderItem>> GetItemsByTenderIdAsync(int tenderId);

        public Task<TenderItem> CreateTenderItemAsync(TenderItem tenderItem);

        public Task UpdateTenderItemAsync(TenderItem tenderItem);

        public Task DeleteTenderItemAsync(TenderItem tenderItem);
    }
}
