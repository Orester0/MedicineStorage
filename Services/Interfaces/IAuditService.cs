using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.Implementations;

namespace MedicineStorage.Services.Interfaces
{
    public interface IAuditService
    {
        public Task<ServiceResult<Audit>> CreateAuditAsync(int userId, CreateAuditRequest request);
        public Task<ServiceResult<Audit>> StartAuditAsync(int userId, StartAuditRequest request);

        public Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int userId, UpdateAuditItemsRequest request);

        public Task<ServiceResult<Audit>> CloseAuditAsync(int userId, CloseAuditRequest request);

        public Task<ServiceResult<IEnumerable<Audit>>> GetAllAuditsAsync();

        public Task<ServiceResult<Audit>> GetAuditByIdAsync(int auditId);

        public Task<ServiceResult<Audit>> CreateAuditAsync(Audit audit);

        public Task<ServiceResult<Audit>> UpdateAuditAsync(Audit audit);

        public Task<ServiceResult<bool>> DeleteAuditAsync(int auditId);
    }
}
