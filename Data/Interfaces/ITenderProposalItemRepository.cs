using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalItemRepository
    {
        public Task<TenderProposalItem?> GetByIdAsync(int id);

        public Task<IEnumerable<TenderProposalItem>> GetAllAsync();

        public Task<IEnumerable<TenderProposalItem>> GetItemsByProposalIdAsync(int proposalId);

        public Task<TenderProposalItem> CreateTenderProposalItemAsync(TenderProposalItem tenderProposalItem);

        public void UpdateTenderProposalItem(TenderProposalItem tenderProposalItem);

        public void DeleteTenderProposalItem(TenderProposalItem tenderProposalItem);


    }
}
