using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRequestRepository
    {
        public Task<MedicineRequest?> GetByIdAsync(int id);
        public Task<(IEnumerable<MedicineRequest>, int)> GetAllAsync(MedicineRequestParams parameters);
        public Task<List<MedicineRequest>> GetRequestsRequestedByUserIdAsync(int userId);
        public Task<MedicineRequest?> GetRequestByUsageIdAsync(int usageId);
        public Task<List<MedicineRequest>> GetRequestsApprovedByUserIdAsync(int userId);
        public Task<List<MedicineRequest>> GetRequestsForMedicineIdAsync(int medicineId);



        public Task<MedicineRequest> CreateRequestAsync(MedicineRequest request);
        public Task<MedicineRequest> UpdateRequestAsync(MedicineRequest request);
        public Task<bool> DeleteRequestAsync(int id);
    }
}
