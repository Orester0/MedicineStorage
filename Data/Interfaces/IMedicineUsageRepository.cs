using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineUsageRepository : IGenericRepository<MedicineUsage>
    {
        public Task<(IEnumerable<MedicineUsage>, int)> GetByParams(MedicineUsageParams parameters);
        public Task<List<MedicineUsage>> GetUsagesByUserIdAsync(int userId);
    }
}
