using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IAuditRepository
    {
        Task<Audit> GetAuditByIdAsync(int auditId);
        Task<IEnumerable<Audit>> GetAllAuditsAsync();
        Task<Audit> CreateAuditAsync(Audit audit);
        Task<Audit> UpdateAuditAsync(Audit audit);
        Task DeleteAuditAsync(int auditId);



        Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId);
        Task<AuditItem> CreateAuditItemAsync(AuditItem auditItem);
        Task<AuditItem> UpdateAuditItemAsync(AuditItem auditItem);
        Task DeleteAuditItemAsync(int auditItemId);
    }
}
