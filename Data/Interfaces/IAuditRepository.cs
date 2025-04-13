using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IAuditRepository : IGenericRepository<Audit>
    {
        Task<(IEnumerable<Audit>, int)> GetByParams(AuditParams auditParams);
        Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId);
        Task<IEnumerable<Audit>> GetAuditsByPlannedUserIdAsync(int userId);
        Task<IEnumerable<Audit>> GetAuditsByExecutedUserIdAsync(int userId);



        Task<IEnumerable<Audit>> GetByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditItem>> CreateAuditItemsAsync(IEnumerable<AuditItem> auditItems);
        void UpdateAuditItem(AuditItem auditItem);
    }
}
