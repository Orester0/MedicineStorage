using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineUsageRepository
    {
        public Task<MedicineUsage?> GetByIdAsync(int id);

        public Task<List<MedicineUsage>> GetAllAsync();

        public Task<List<MedicineUsage>> GetUsagesByMedicineAsync(int medicineId);

        public Task<List<MedicineUsage>> GetUsagesByUserAsync(int userId);

        public Task<List<MedicineUsage>> GetUsagesByDateRangeAsync(DateTime startDate, DateTime endDate);

        public Task<MedicineUsage> AddUsageAsync(MedicineUsage usage);

        public Task<MedicineUsage> UpdateUsageAsync(MedicineUsage usage);

        public Task<bool> DeleteUsageAsync(int id);

        public Task<decimal> GetTotalUsageQuantityAsync(int medicineId, DateTime? startDate = null, DateTime? endDate = null);

    }
}
