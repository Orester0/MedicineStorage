
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        Task<IEnumerable<Medicine>> GetAllRequiringAuditAsync();
        public Task<List<Medicine>> GetByIdsAsync(IEnumerable<int> medicineIds);
        public Task<(IEnumerable<Medicine>, int)> GetByParams(MedicineParams parameters);
      
    }
}
