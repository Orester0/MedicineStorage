using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalRepository
    {
        Task<TenderProposal?> GetByIdAsync(int id);
        Task<List<TenderProposal>> GetAllProposalsForTenderAsync(int tenderId);
        Task<List<TenderProposal>> GetProposalsByDistributorAsync(int distributorId);
        Task<List<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status);
        Task<bool> SubmitProposalAsync(TenderProposal proposal);
        Task<bool> UpdateProposalStatusAsync(int proposalId, ProposalStatus status);
        Task<bool> DeleteProposalAsync(int id);
        Task<bool> AddProposalItemAsync(TenderProposalItem item);
        Task<bool> UpdateProposalItemAsync(TenderProposalItem item);
        Task<bool> RemoveProposalItemAsync(int itemId);
        Task<TenderProposal?> GetWinningProposalForTenderAsync(int tenderId);
    }
}
