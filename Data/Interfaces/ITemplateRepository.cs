namespace MedicineStorage.Data.Interfaces
{
    public interface ITemplateRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetByUserIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllActiveAndDueAsync();
        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task DeleteAsync(int id);
    }
}
