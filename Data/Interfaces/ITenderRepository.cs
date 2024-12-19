using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderRepository
    {
        Task<Tender> GetByIdAsync(int id);
        public Task<(IEnumerable<Tender>, int)> GetAllTendersAsync(TenderParams tenderParams);
        Task<Tender> CreateTenderAsync(Tender tender);
        Task UpdateTenderAsync(Tender tender);
        Task DeleteTenderAsync(int id);
        Task<IEnumerable<Tender>> GetTendersCreatedByUserIdAsync(int userId);
        Task<IEnumerable<Tender>> GetTendersAwardedByUserIdAsync(int userId);

        Task<Tender> GetTenderByProposalIdAsync(int proposalId);
    }
}
