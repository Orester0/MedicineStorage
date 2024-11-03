using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Interfaces
{
    public interface ITenderRepository
    {
        Task<Tender> GetByIdAsync(int id);
        Task<IEnumerable<Tender>> GetAllAsync();
        Task AddAsync(Tender entity);
        Task Update(Tender entity);
        Task DeleteAsync(Tender entity);
    }
}
