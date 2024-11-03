using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineRepository
    {
        Task<Medicine> GetByIdAsync(int id);
        Task<IEnumerable<Medicine>> GetAllAsync();
        Task AddAsync(Medicine entity);
        Task Update(Medicine entity);
        Task DeleteAsync(Medicine entity);
    }
}
