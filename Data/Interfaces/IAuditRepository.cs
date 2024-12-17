using MedicineStorage.Models.AuditModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IAuditRepository
    {
        public Task<IEnumerable<Audit>> GetAllAuditsAsync();

        public Task<Audit?> GetAuditByIdAsync(int auditId);

        public Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId);

        public Task AddAsync(Audit audit);

        public Task UpdateAsync(Audit audit);

        public Task DeleteAsync(int auditId);

        public Task AddAuditItemAsync(AuditItem auditItem);

        public Task UpdateAuditItemAsync(AuditItem auditItem);

        public Task DeleteAuditItemAsync(int auditItemId);
    }
}
