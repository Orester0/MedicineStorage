using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRepository
    {
        public Task<List<Medicine>> GetByIdsAsync(IEnumerable<int> medicineIds);
        public Task<Medicine?> GetByIdAsync(int id);
        public Task<(IEnumerable<Medicine>, int)> GetAllAsync(MedicineParams parameters);
        public Task<List<Medicine>> GetAllAsync();
        public Task<Medicine> AddAsync(Medicine medicine);
        public void Update(Medicine medicine);
        public void DeleteAsync(Medicine medicine);
    }
}
