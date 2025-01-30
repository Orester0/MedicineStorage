using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalRepository
    {
        public Task<TenderProposal> GetByIdAsync(int id);
        public Task<IEnumerable<TenderProposal>> GetAllAsync();
        public Task<TenderProposal> CreateTenderProposalAsync(TenderProposal tenderProposal);
        public void UpdateTenderProposal(TenderProposal tenderProposal);
        public Task DeleteTenderProposalAsync(int id);
        public Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId);
        public Task<IEnumerable<TenderProposal>> GetProposalsCreatedByUserIdAsync(int distributorId);
        public Task<IEnumerable<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status);
    }
}
