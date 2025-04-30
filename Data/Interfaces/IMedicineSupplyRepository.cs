using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineSupplyRepository : IGenericRepository<MedicineSupply>
    {
        Task<List<MedicineSupply>> GetSuppliesByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate);
        Task<Dictionary<int, List<MedicineSupply>>> GetAllMedicineSuppliesInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<(IEnumerable<MedicineSupply>, int)> GetByParamsAsync(MedicineSupplyParams parameters);
    }
}
