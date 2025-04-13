using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
