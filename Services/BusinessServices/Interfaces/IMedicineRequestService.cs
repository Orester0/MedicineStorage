using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IMedicineRequestService
    {
        public Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByIdAsync(int id);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsRequestedByUserId(int userId);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsApprovedByUserId(int userId);
        public Task<ServiceResult<PagedList<ReturnMedicineRequestDTO>>> GetAllRequestsAsync(MedicineRequestParams parameters);


        public Task<ServiceResult<ReturnMedicineRequestDTO>> CreateRequestAsync(CreateMedicineRequestDTO createRequestDTO, int userId);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> ApproveRequestAsync(int requestId, int userId, bool isSpecialApproval = false);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> RejectRequestAsync(int requestId, int userId, bool isSpecialApproval = false);
        public Task<ServiceResult<bool>> DeleteRequestAsync(int requestId, int userId);
    }
}
