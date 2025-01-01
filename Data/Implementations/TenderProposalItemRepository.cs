using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderProposalItemRepository(AppDbContext _context) : ITenderProposalItemRepository
    {
        public async Task<TenderProposalItem?> GetByIdAsync(int id)
        {
            return await _context.TenderProposalItems
                .Include(tpi => tpi.Medicine)
                .FirstOrDefaultAsync(tpi => tpi.Id == id);
        }

        public async Task<IEnumerable<TenderProposalItem>> GetAllAsync()
        {
            return await _context.TenderProposalItems.ToListAsync();
        }

        public async Task<IEnumerable<TenderProposalItem>> GetItemsByProposalIdAsync(int proposalId)
        {
            return await _context.TenderProposalItems
                .Where(tpi => tpi.TenderProposalId == proposalId)
                .Include(tpi => tpi.Medicine)
                .ToListAsync();
        }

        public async Task<TenderProposalItem> CreateTenderProposalItemAsync(TenderProposalItem tenderProposalItem)
        {
            await _context.TenderProposalItems.AddAsync(tenderProposalItem);
            return tenderProposalItem;
        }

        public void UpdateTenderProposalItem(TenderProposalItem tenderProposalItem)
        {
            _context.TenderProposalItems.Update(tenderProposalItem);
        }

        public void DeleteTenderProposalItem(TenderProposalItem tenderProposalItem)
        {
            _context.TenderProposalItems.Remove(tenderProposalItem);
        }

    }
}
