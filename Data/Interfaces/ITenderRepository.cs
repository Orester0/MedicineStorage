using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderRepository : IGenericRepository<Tender>
    {

        public Task<(IEnumerable<Tender>, int)> GetByParams(TenderParams tenderParams);
        Task<IEnumerable<Tender>> GetTendersCreatedByUserIdAsync(int userId);
        Task<IEnumerable<Tender>> GetTendersAwardedByUserIdAsync(int userId);
        Task<Tender> GetTenderByProposalIdAsync(int proposalId);
    }
}
