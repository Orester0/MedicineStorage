using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineUsageRepository
    {
        Task<MedicineUsage> GetByIdAsync(int id);
        Task<IEnumerable<MedicineUsage>> GetAllAsync();
        Task AddAsync(MedicineUsage entity);
        Task Update(MedicineUsage entity);
        Task DeleteAsync(MedicineUsage entity);
    }
}
