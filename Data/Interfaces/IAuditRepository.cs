using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IAuditRepository : IGenericRepository<Audit>
    {
        Task<(IEnumerable<Audit>, int)> GetByParams(AuditParams auditParams);
        public Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId);
        public Task<IEnumerable<Audit>> GetAuditsByPlannedUserIdAsync(int userId);
        public Task<IEnumerable<Audit>> GetAuditsByExecutedUserIdAsync(int userId);


       
        public Task<IEnumerable<AuditItem>> CreateAuditItemsAsync(IEnumerable<AuditItem> auditItems);
        public void UpdateAuditItem(AuditItem auditItem);
    }
}
