using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderItemRepository : IGenericRepository<TenderItem>
    {
        public Task<IEnumerable<TenderItem>> GetItemsByTenderIdAsync(int tenderId);

    }
}
