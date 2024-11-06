using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineUsageRepository
    {
        Task<MedicineUsage?> GetByIdAsync(int id);
        Task<List<MedicineUsage>> GetAllAsync();
        Task<List<MedicineUsage>> GetUsagesByMedicineAsync(int medicineId);
        Task<List<MedicineUsage>> GetUsagesByUserAsync(int userId);
        Task<List<MedicineUsage>> GetUsagesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> RecordUsageAsync(MedicineUsage usage);
        Task<bool> UpdateUsageAsync(MedicineUsage usage);
        Task<bool> DeleteUsageAsync(int id);
        Task<decimal> GetTotalUsageForMedicineAsync(int medicineId, DateTime? startDate = null, DateTime? endDate = null);

    }
}
