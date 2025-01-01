﻿using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineUsageRepository(AppDbContext _context) : IMedicineUsageRepository
    {
        public async Task<MedicineUsage?> GetByIdAsync(int id)
        {
            return await _context.MedicineUsages
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .Include(u => u.MedicineRequest)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<(IEnumerable<MedicineUsage>, int)> GetAllAsync(MedicineUsageParams parameters)
        {
            var query = _context.MedicineUsages
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .Include(u => u.MedicineRequest)
                .AsQueryable();

            // Фільтрація
            if (parameters.FromDate.HasValue)
                query = query.Where(u => u.UsageDate >= parameters.FromDate);

            if (parameters.ToDate.HasValue)
                query = query.Where(u => u.UsageDate <= parameters.ToDate);

            if (parameters.MedicineId.HasValue)
                query = query.Where(u => u.MedicineId == parameters.MedicineId);

            if (parameters.UsedByUserId.HasValue)
                query = query.Where(u => u.UsedByUserId == parameters.UsedByUserId);

            if (parameters.MedicineRequestId.HasValue)
                query = query.Where(u => u.MedicineRequestId == parameters.MedicineRequestId);

            if (parameters.MinQuantity.HasValue)
                query = query.Where(u => u.Quantity >= parameters.MinQuantity);

            if (parameters.MaxQuantity.HasValue)
                query = query.Where(u => u.Quantity <= parameters.MaxQuantity);

            if (!string.IsNullOrWhiteSpace(parameters.Notes))
                query = query.Where(u => u.Notes != null && u.Notes.Contains(parameters.Notes));

            // Сортування
            query = parameters.SortBy?.ToLower() switch
            {
                "usagedate" => parameters.IsDescending
                    ? query.OrderByDescending(u => u.UsageDate)
                    : query.OrderBy(u => u.UsageDate),
                "quantity" => parameters.IsDescending
                    ? query.OrderByDescending(u => u.Quantity)
                    : query.OrderBy(u => u.Quantity),
                _ => parameters.IsDescending
                    ? query.OrderByDescending(u => u.UsageDate)
                    : query.OrderBy(u => u.UsageDate)
            };

            // Пагінація
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<List<MedicineUsage>> GetUsagesByMedicineIdAsync(int medicineId)
        {
            return await _context.MedicineUsages
                .Where(u => u.MedicineId == medicineId)
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .Include(u => u.MedicineRequest)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByUserIdAsync(int userId)
        {
            return await _context.MedicineUsages
                .Where(u => u.UsedByUserId == userId)
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .Include(u => u.MedicineRequest)
                .ToListAsync();
        }


        public async Task<List<MedicineUsage>> GetUsagesByRequestIdAsync(int requestId)
        {
            return await _context.MedicineUsages
                .Where(u => u.MedicineRequestId == requestId)
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .Include(u => u.MedicineRequest)
                .ToListAsync();
        }

        public async Task<MedicineUsage> CreateUsageAsync(MedicineUsage usage)
        {
            await _context.MedicineUsages.AddAsync(usage);
            return usage;
        }

        public void UpdateUsage(MedicineUsage usage)
        {
            _context.MedicineUsages.Update(usage);
        }

        public async Task DeleteUsageAsync(int usageId)
        {
            var usage = await _context.MedicineUsages.FindAsync(usageId);
            if (usage != null)
            {
                _context.MedicineUsages.Remove(usage);
            }
        }



    }
}