using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineUsageRepository
    {
        public Task<MedicineUsage?> GetByIdAsync(int id);

        public Task<(IEnumerable<MedicineUsage>, int)> GetAllAsync(MedicineUsageParams parameters);

        public Task<List<MedicineUsage>> GetUsagesByMedicineIdAsync(int medicineId);

        public Task<List<MedicineUsage>> GetUsagesByUserIdAsync(int userId);

        public Task<MedicineUsage> CreateUsageAsync(MedicineUsage usage);

        public Task<MedicineUsage> UpdateUsageAsync(MedicineUsage usage);

        public Task<bool> DeleteUsageAsync(int id);

        public Task<List<MedicineUsage>> GetUsagesByRequestIdAsync(int requestId);

    }
}
