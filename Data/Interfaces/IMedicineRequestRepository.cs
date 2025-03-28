using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRequestRepository : IGenericRepository<MedicineRequest>
    {

        public Task<(IEnumerable<MedicineRequest>, int)> GetByParams(MedicineRequestParams parameters);
        public Task<List<MedicineRequest>> GetByMedicineIdAsync(int medicineId);
        public Task<List<MedicineRequest>> GetRequestsRequestedByUserIdAsync(int userId);

        public Task<List<MedicineRequest>> GetRequestsApprovedByUserIdAsync(int userId);
        
    }
}
