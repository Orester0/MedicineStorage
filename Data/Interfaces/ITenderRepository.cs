using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderRepository
    {
        Task<Tender?> GetByIdAsync(int id);
        Task<List<Tender>> GetAllAsync();
        Task<List<Tender>> GetTendersByStatusAsync(TenderStatus status);
        Task<List<Tender>> GetActiveTendersAsync();
        Task<bool> CreateTenderAsync(Tender tender);
        Task<bool> UpdateTenderAsync(Tender tender);
        Task<bool> DeleteTenderAsync(int id);
        Task<bool> PublishTenderAsync(int id);
        Task<bool> CloseTenderAsync(int id);
        Task<bool> CancelTenderAsync(int id);
        Task<bool> AddTenderItemAsync(TenderItem item);
        Task<bool> UpdateTenderItemAsync(TenderItem item);
        Task<bool> RemoveTenderItemAsync(int itemId);
        Task<List<Tender>> GetTendersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
