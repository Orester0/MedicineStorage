using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderProposalItemRepository : IGenericRepository<TenderProposalItem>
    {
        public Task<IEnumerable<TenderProposalItem>> GetItemsByProposalIdAsync(int proposalId);


    }
}
