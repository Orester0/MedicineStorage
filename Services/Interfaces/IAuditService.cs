using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface IAuditService
    {
        public Task<ServiceResult<Audit>> StartAuditAsync(int userId, int[] medicineIds);

        public Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int auditId, Dictionary<int, decimal> actualQuantities, int userId);

        public Task<ServiceResult<Audit>> CloseAuditAsync(int auditId, int userId);

        public Task<ServiceResult<IEnumerable<Audit>>> GetAllAuditsAsync();

        public Task<ServiceResult<Audit>> GetAuditByIdAsync(int auditId);

        public Task<ServiceResult<Audit>> CreateAuditAsync(Audit audit);

        public Task<ServiceResult<Audit>> UpdateAuditAsync(Audit audit);

        public Task<ServiceResult<bool>> DeleteAuditAsync(int auditId);

    }
}
