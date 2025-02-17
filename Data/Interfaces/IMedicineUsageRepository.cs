using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineUsageRepository
    {
        public Task<MedicineUsage?> GetByIdAsync(int id);
        public Task<(IEnumerable<MedicineUsage>, int)> GetAllAsync(MedicineUsageParams parameters);
        public Task<List<MedicineUsage>> GetUsagesByUserIdAsync(int userId);


        public Task<MedicineUsage> CreateUsageAsync(MedicineUsage usage);
        public void UpdateUsage(MedicineUsage usage);
        public Task DeleteUsageAsync(int id);
    }
}
