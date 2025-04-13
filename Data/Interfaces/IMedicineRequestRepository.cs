using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRequestRepository : IGenericRepository<MedicineRequest>
    {

        Task<IEnumerable<MedicineRequest>> GetByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate);
        Task<(IEnumerable<MedicineRequest>, int)> GetByParams(MedicineRequestParams parameters);
        Task<List<MedicineRequest>> GetByMedicineIdAsync(int medicineId);
        Task<List<MedicineRequest>> GetRequestsRequestedByUserIdAsync(int userId);
        Task<List<MedicineRequest>> GetRequestsApprovedByUserIdAsync(int userId);
        
    }
}
