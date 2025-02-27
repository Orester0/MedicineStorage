using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IMedicineRequestService
    {
        public Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByIdAsync(int id);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsRequestedByUserId(int userId);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsApprovedByUserId(int userId);
        public Task<ServiceResult<PagedList<ReturnMedicineRequestDTO>>> GetPaginatedAudits(MedicineRequestParams parameters);


        public Task<ServiceResult<ReturnMedicineRequestDTO>> CreateRequestAsync(CreateMedicineRequestDTO createRequestDTO, int userId);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> ApproveRequestAsync(int requestId, int userId, List<string> userRoles);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> RejectRequestAsync(int requestId, int userId, List<string> userRoles);
        public Task<ServiceResult<bool>> DeleteRequestAsync(int requestId, int userId, List<string> userRoles);
    }
}
