using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalRepository
    {
        Task<TenderProposal> GetProposalByIdAsync(int id);
        Task<IEnumerable<TenderProposal>> GetAllAsync();
        Task<TenderProposal> CreateTenderProposalAsync(TenderProposal tenderProposal);
        Task UpdateTenderProposalAsync(TenderProposal tenderProposal);
        Task DeleteTenderProposalAsync(int id);
        Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId);
        Task<IEnumerable<TenderProposal>> GetProposalsCreatedByUserIdAsync(int distributorId);
        Task<IEnumerable<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status);
    }
}
