using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRequestRepository
    {
        Task<MedicineRequest?> GetByIdAsync(int id);
        Task<List<MedicineRequest>> GetAllAsync();
        Task<List<MedicineRequest>> GetPendingRequestsAsync();
        Task<List<MedicineRequest>> GetRequestsByUserAsync(int userId);
        Task<List<MedicineRequest>> GetRequestsByStatusAsync(RequestStatus status);
        Task<bool> CreateRequestAsync(MedicineRequest request);
        Task<bool> UpdateRequestStatusAsync(int requestId, RequestStatus status, int? approvedByUserId = null);
        Task<bool> DeleteRequestAsync(int id);
    }
}
