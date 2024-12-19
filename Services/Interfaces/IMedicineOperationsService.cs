using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineOperationsService
    {
        public Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByIdAsync(int id);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsRequestedByUserId(int userId);
        public Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsageByIdAsync(int id);
        public Task<ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>> GetUsagesByRequestIdAsync(int requestId);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByUsageIdAsync(int usageId);
        public Task<ServiceResult<PagedList<ReturnMedicineRequestDTO>>> GetAllRequestsAsync(MedicineRequestParams parameters);
        public Task<ServiceResult<PagedList<ReturnMedicineUsageDTO>>> GetAllUsagesAsync(MedicineUsageParams parameters);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsApprovedByUserId(int userId);
        public Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsForMedicineId(int medicineId);




        public Task<ServiceResult<ReturnMedicineRequestDTO>> CreateRequestAsync(CreateMedicineRequestDTO createRequestDTO, int userId);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> ApproveRequestAsync(int requestId, int userId, bool isSpecialApproval = false);
        public Task<ServiceResult<ReturnMedicineUsageDTO>> CreateUsageAsync(CreateMedicineUsageDTO createUsageDTO, int userId);
        public Task<ServiceResult<ReturnMedicineRequestDTO>> RejectRequestAsync(int requestId, int userId, bool isSpecialApproval = false);
        public Task<ServiceResult<bool>> DeleteRequestAsync(int requestId);
    }
}
