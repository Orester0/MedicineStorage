using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderRepository
    {
        Task<Tender> GetByIdAsync(int id);
        Task<IEnumerable<Tender>> GetAllAsync();
        Task<Tender> AddAsync(Tender tender);
        Task UpdateAsync(Tender tender);
        Task DeleteAsync(int id);
        Task<IEnumerable<Tender>> GetTendersByStatusAsync(TenderStatus status);
        Task<IEnumerable<Tender>> GetTendersByUserAsync(int userId);
    }
}
