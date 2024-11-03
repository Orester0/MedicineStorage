using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock> GetByIdAsync(int id);
        Task<IEnumerable<Stock>> GetAllAsync();
        Task AddAsync(Stock entity);
        Task Update(Stock entity);
        Task DeleteAsync(Stock entity);
    }
}
