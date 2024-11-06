using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class MedicineRepository(AppDbContext _context) : IMedicineRepository
    {
        public async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _context.Medicines
                .Include(m => m.Requests)
                .Include(m => m.UsageRecords)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            return await _context.Medicines
                .ToListAsync();
        }

        public async Task<List<Medicine>> GetLowStockMedicinesAsync()
        {
            return await _context.Medicines
                .Where(m => m.Stock <= m.MinimumStock)
                .ToListAsync();
        }

        public async Task<List<Medicine>> GetMedicinesRequiringAuditAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Medicines
                .Where(m => m.RequiresStrictAudit)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Medicine medicine)
        {
            try
            {
                _context.Medicines.Add(medicine);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Medicine medicine)
        {
            try
            {
                _context.Medicines.Update(medicine);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return false;

            try
            {
                _context.Medicines.Remove(medicine);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task UpdateStockAsync(int medicineId, decimal quantity)
        {
            var medicine = await GetByIdAsync(medicineId);
            if (medicine != null)
            {
                medicine.Stock = quantity;
                await UpdateAsync(medicine);
            }
        }
        public async Task<decimal> GetCurrentStockLevelAsync(int medicineId)
        {
            var medicine = await GetByIdAsync(medicineId);
            return medicine?.Stock ?? 0;
        }

    }
}
