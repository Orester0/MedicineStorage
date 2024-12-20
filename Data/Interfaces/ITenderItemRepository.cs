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

        public void UpdateTenderItem(TenderItem tenderItem);

        public void DeleteTenderItem(TenderItem tenderItem);
    }
}
