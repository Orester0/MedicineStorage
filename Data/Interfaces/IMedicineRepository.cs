using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRepository
    {

        public Task<Medicine?> GetByIdAsync(int id);

        public Task<(IEnumerable<Medicine>, int)> GetAllAsync(MedicineParams parameters);

        public Task<Medicine?> CreateMedicineAsync(Medicine medicine);

        public Task UpdateMedicineAsync(Medicine medicine);

        public Task DeleteMedicineAsync(Medicine medicine);
    }
}
