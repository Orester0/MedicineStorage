using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IAuditService
    {

        public Task<ServiceResult<IEnumerable<ReturnAuditDTO>>> GetAuditsExecutedByUserId(int userId);
        public Task<ServiceResult<IEnumerable<ReturnAuditDTO>>> GetAuditsPlannedByUserId(int userId);
        public Task<ServiceResult<PagedList<ReturnAuditDTO>>> GetPaginatedAudits(AuditParams auditParams);
        public Task<ServiceResult<ReturnAuditDTO>> GetAuditByIdAsync(int auditId);




        public Task<ServiceResult<Audit>> CreateAuditAsync(int userId, CreateAuditDTO request);
        public Task<ServiceResult<Audit>> StartAuditAsync(int userId, int auditId, CreateAuditNoteDTO request);
        public Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int userId, int auditId, UpdateAuditItemsRequest request);
        public Task<ServiceResult<Audit>> CloseAuditAsync(int userId, int auditId, CreateAuditNoteDTO request);
        public Task<ServiceResult<bool>> DeleteAuditAsync(int auditId, int userId, List<string> userRoles);
    }
}
