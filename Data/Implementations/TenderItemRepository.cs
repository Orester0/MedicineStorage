﻿using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderItemRepository(AppDbContext _context) : ITenderItemRepository
    {
        public async Task<TenderItem?> GetByIdAsync(int id)
        {
            return await _context.TenderItems
                .Include(ti => ti.Tender)
                .Include(ti => ti.Medicine)
                .FirstOrDefaultAsync(ti => ti.Id == id);
                
        }

        public async Task<IEnumerable<TenderItem>> GetAllAsync()
        {
            return await _context.TenderItems.ToListAsync();
        }

        public async Task<IEnumerable<TenderItem>> GetByTenderIdAsync(int tenderId)
        {
            return await _context.TenderItems
                .Where(ti => ti.TenderId == tenderId)
                .ToListAsync();
        }

        public async Task<TenderItem> AddAsync(TenderItem tenderItem)
        {
            await _context.TenderItems.AddAsync(tenderItem);
            return tenderItem;
        }

        public async Task UpdateAsync(TenderItem tenderItem)
        {
            _context.TenderItems.Update(tenderItem);
        }

        public async Task DeleteAsync(TenderItem tenderItem)
        {
            _context.TenderItems.Remove(tenderItem);
        }
    }
}
