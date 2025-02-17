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


        public Task<List<MedicineRequest>> GetRequestsApprovedByUserIdAsync(int userId);
        public Task<MedicineRequest> AddAsync(MedicineRequest request);
        public void Update(MedicineRequest request);
        public Task DeleteAsync(int requestId);
    }
}
