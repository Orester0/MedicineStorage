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



        public Task CreateAuditAsync(Audit audit);

        public Task UpdateAuditAsync(Audit audit);

        public Task DeleteAuditAsync(int auditId);

        public Task AddAuditItemAsync(AuditItem auditItem);

        public Task UpdateAuditItemAsync(AuditItem auditItem);

        public Task DeleteAuditItemAsync(int auditItemId);
    }
}
