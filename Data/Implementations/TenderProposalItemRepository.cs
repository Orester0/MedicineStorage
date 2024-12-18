﻿using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderProposalItemRepository(AppDbContext _context) : ITenderProposalItemRepository
    {
        public async Task<TenderProposalItem?> GetByIdAsync(int id)
        {
            return await _context.TenderProposalItems
                .Include(tpi => tpi.Proposal)
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
                .ToListAsync();
        }

        public async Task<TenderProposalItem> CreateTenderProposalItemAsync(TenderProposalItem tenderProposalItem)
        {
            _context.TenderProposalItems.Add(tenderProposalItem);
            await _context.SaveChangesAsync();
            return tenderProposalItem;
        }

        public async Task UpdateTenderProposalItemAsync(TenderProposalItem tenderProposalItem)
        {
            _context.TenderProposalItems.Update(tenderProposalItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTenderProposalItemAsync(TenderProposalItem tenderProposalItem)
        {
            _context.TenderProposalItems.Remove(tenderProposalItem);
            await _context.SaveChangesAsync();
        }

    }
}
