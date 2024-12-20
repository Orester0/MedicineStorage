using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.AuditModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IAuditRepository
    {
        Task<(IEnumerable<Audit>, int)> GetAllAuditsAsync(AuditParams auditParams);

        public Task<Audit?> GetAuditByIdAsync(int auditId);

        public Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId);


        public Task<IEnumerable<Audit>> GetAuditsByPlannedUserIdAsync(int userId);


        public Task<IEnumerable<Audit>> GetAuditsByExecutedUserIdAsync(int userId);



        public Task<Audit> CreateAuditAsync(Audit audit);

        public void UpdateAudit(Audit audit);

        public Task DeleteAuditAsync(int auditId);

        public Task<AuditItem> CreateAuditItemAsync(AuditItem auditItem);

        public void UpdateAuditItem(AuditItem auditItem);

        public Task DeleteAuditItemAsync(int auditItemId);
    }
}
