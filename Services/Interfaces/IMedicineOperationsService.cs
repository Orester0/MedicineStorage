using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineOperationsService
    {
        public Task<ServiceResult<IEnumerable<MedicineRequestDTO>>> GetAllRequestsAsync();

        public Task<ServiceResult<MedicineRequest>> CreateRequestAsync(CreateMedicineRequestDTO createRequestDTO, int userId);

        public Task<ServiceResult<MedicineUsage>> CreateUsageAsync(CreateMedicineUsageDTO createUsageDTO, int userId);

        public Task<ServiceResult<MedicineRequestDTO>> ApproveRequestAsync(int requestId, int userId);

        public Task<ServiceResult<IEnumerable<MedicineUsageDTO>>> GetAllUsagesAsync();


        public Task<ServiceResult<MedicineRequestDTO>> ProcessSpecialApprovalRequestAsync(int requestId, bool isApproved, int userId);

        public Task<ServiceResult<MedicineRequestDTO>> GetRequestByIdAsync(int id);

        public Task<ServiceResult<IEnumerable<MedicineRequestDTO>>> GetRequestsByUserAsync(int userId);

        public Task<ServiceResult<IEnumerable<MedicineRequestDTO>>> GetRequestsByStatusAsync(RequestStatus status);

        public Task<ServiceResult<MedicineRequestDTO>> UpdateRequestStatusAsync(int requestId, RequestStatus newStatus, int userId);

        public Task<ServiceResult<bool>> DeleteRequestAsync(int requestId);

        public Task<ServiceResult<MedicineUsageDTO>> GetUsageByIdAsync(int id);
    }
}
