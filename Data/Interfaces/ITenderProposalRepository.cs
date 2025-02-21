using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalRepository : IGenericRepository<TenderProposal>
    {




        public Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId);
        public Task<IEnumerable<TenderProposal>> GetProposalsCreatedByUserIdAsync(int distributorId);
        public Task<IEnumerable<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status);
    }
}
