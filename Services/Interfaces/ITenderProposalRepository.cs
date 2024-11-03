using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Interfaces
{
    public interface ITenderProposalRepository
    {
        Task<TenderProposal> GetByIdAsync(int id);
        Task<IEnumerable<TenderProposal>> GetAllAsync();
        Task AddAsync(TenderProposal entity);
        Task Update(TenderProposal entity);
        Task DeleteAsync(TenderProposal entity);
    }
}
