
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        Task<bool> IsCategoryUnusedAsync(int categoryId);
        Task DeleteCategoryAsync(int categoryId);
        Task<MedicineCategory?> GetCategoryByNameAsync(string name);
        Task<MedicineCategory> GetOrCreateCategoryAsync(string name);
        Task<List<MedicineCategory>> GetAllCategoriesAsync();


        Task<IEnumerable<Medicine>> GetAllRequiringAuditAsync();
        Task<List<Medicine>> GetByIdsAsync(IEnumerable<int> medicineIds);
        Task<(IEnumerable<Medicine>, int)> GetByParams(MedicineParams parameters);
      
    }
}
