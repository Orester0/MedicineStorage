using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalItemRepository
    {
        public Task<TenderProposalItem?> GetByIdAsync(int id);

        public Task<IEnumerable<TenderProposalItem>> GetAllAsync();

        public Task<IEnumerable<TenderProposalItem>> GetByProposalIdAsync(int proposalId);

        public Task<TenderProposalItem> AddAsync(TenderProposalItem tenderProposalItem);

        public Task UpdateAsync(TenderProposalItem tenderProposalItem);

        public Task DeleteAsync(TenderProposalItem tenderProposalItem);
    }
}
