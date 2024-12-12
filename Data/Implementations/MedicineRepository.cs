using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data.Implementations
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

        public async Task<IEnumerable<Medicine>> GetAllAsync()
        {
            return await _context.Medicines.ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync()
        {
            return await _context.Medicines
                .Where(m => m.Stock <= m.MinimumStock)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetMedicinesRequiringAuditAsync()
        {
            return await _context.Medicines
                .Where(m => m.RequiresStrictAudit)
                .ToListAsync();
        }

        public async Task<Medicine?> AddAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            return medicine;
        }

        public void Update(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
        }

        public void Delete(Medicine medicine)
        {
            _context.Medicines.Remove(medicine);
        }

    }
}
