using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalRepository
    {
        Task<TenderProposal> GetByIdAsync(int id);
        Task<IEnumerable<TenderProposal>> GetAllAsync();
        Task<TenderProposal> AddAsync(TenderProposal tenderProposal);
        Task UpdateAsync(TenderProposal tenderProposal);
        Task DeleteAsync(int id);
        Task<IEnumerable<TenderProposal>> GetProposalsByTenderAsync(int tenderId);
        Task<IEnumerable<TenderProposal>> GetProposalsByDistributorAsync(int distributorId);
        Task<IEnumerable<TenderProposal>> GetProposalsByStatusAsync(ProposalStatus status);
    }
}
