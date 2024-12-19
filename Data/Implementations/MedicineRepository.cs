using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

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

        public async Task<(IEnumerable<Medicine>, int)> GetAllAsync(MedicineParams parameters)
        {
            var query = _context.Medicines.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Name))
                query = query.Where(m => m.Name.Contains(parameters.Name));

            if (!string.IsNullOrEmpty(parameters.Description))
                query = query.Where(m => m.Description.Contains(parameters.Description));

            if (!string.IsNullOrEmpty(parameters.Category))
                query = query.Where(m => m.Category == parameters.Category);

            if (parameters.RequiresSpecialApproval.HasValue)
                query = query.Where(m => m.RequiresSpecialApproval == parameters.RequiresSpecialApproval);

            if (parameters.MinStock.HasValue)
                query = query.Where(m => m.Stock >= parameters.MinStock);

            if (parameters.MaxStock.HasValue)
                query = query.Where(m => m.Stock <= parameters.MaxStock);

            if (parameters.MinMinimumStock.HasValue)
                query = query.Where(m => m.MinimumStock >= parameters.MinMinimumStock);

            if (parameters.MaxMinimumStock.HasValue)
                query = query.Where(m => m.MinimumStock <= parameters.MaxMinimumStock);

            if (parameters.RequiresStrictAudit.HasValue)
                query = query.Where(m => m.RequiresStrictAudit == parameters.RequiresStrictAudit);

            if (parameters.MinAuditFrequencyDays.HasValue)
                query = query.Where(m => m.AuditFrequencyDays >= parameters.MinAuditFrequencyDays);

            if (parameters.MaxAuditFrequencyDays.HasValue)
                query = query.Where(m => m.AuditFrequencyDays <= parameters.MaxAuditFrequencyDays);

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.IsDescending ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
                    "category" => parameters.IsDescending ? query.OrderByDescending(m => m.Category) : query.OrderBy(m => m.Category),
                    "stock" => parameters.IsDescending ? query.OrderByDescending(m => m.Stock) : query.OrderBy(m => m.Stock),
                    "minimumstock" => parameters.IsDescending ? query.OrderByDescending(m => m.MinimumStock) : query.OrderBy(m => m.MinimumStock),
                    "auditfrequencydays" => parameters.IsDescending ? query.OrderByDescending(m => m.AuditFrequencyDays) : query.OrderBy(m => m.AuditFrequencyDays),
                    _ => query.OrderBy(m => m.Id)
                };
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<Medicine?> CreateMedicineAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            return medicine;
        }

        public async Task UpdateMedicineAsync(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicineAsync(Medicine medicine)
        {
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
        }

    }
}
