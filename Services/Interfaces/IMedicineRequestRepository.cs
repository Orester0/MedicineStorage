using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineRequestRepository
    {
        Task<MedicineRequest> GetByIdAsync(int id);
        Task<IEnumerable<MedicineRequest>> GetAllAsync();
        Task AddAsync(MedicineRequest entity);
        Task Update(MedicineRequest entity);
        Task DeleteAsync(MedicineRequest entity);
    }
}
