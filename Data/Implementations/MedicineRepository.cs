using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRepository(AppDbContext _context) : IMedicineRepository
    {

        public async Task<List<Medicine>> GetByIdsAsync(IEnumerable<int> medicineIds)
        {
            return await _context.Medicines
                .Where(m => medicineIds.Contains(m.Id))
                .ToListAsync();
        }

        public async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _context.Medicines
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            return await _context.Medicines
               .ToListAsync();
        }

        public async Task<(IEnumerable<Medicine>, int)> GetAllAsync(MedicineParams parameters)
        {
            var query = _context.Medicines.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                query = query.Where(m => m.Name.Contains(parameters.Name));


            if (!string.IsNullOrWhiteSpace(parameters.Category))
                query = query.Where(m => m.Category == parameters.Category);

            if (parameters.RequiresSpecialApproval.HasValue)
                query = query.Where(m => m.RequiresSpecialApproval == parameters.RequiresSpecialApproval);

            if (parameters.MinStock.HasValue)
                query = query.Where(m => m.Stock >= parameters.MinStock);

            if (parameters.MaxStock.HasValue)
                query = query.Where(m => m.Stock <= parameters.MaxStock);

            if (parameters.RequiresStrictAudit.HasValue)
                query = query.Where(m => m.RequiresStrictAudit == parameters.RequiresStrictAudit);

            
            query = parameters.SortBy?.ToLower() switch
            {
                "name" => parameters.IsDescending ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
                "category" => parameters.IsDescending ? query.OrderByDescending(m => m.Category) : query.OrderBy(m => m.Category),
                "stock" => parameters.IsDescending ? query.OrderByDescending(m => m.Stock) : query.OrderBy(m => m.Stock),
                "minimumstock" => parameters.IsDescending ? query.OrderByDescending(m => m.MinimumStock) : query.OrderBy(m => m.MinimumStock),
                "auditfrequencydays" => parameters.IsDescending ? query.OrderByDescending(m => m.AuditFrequencyDays) : query.OrderBy(m => m.AuditFrequencyDays),
                _ => query.OrderBy(m => m.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<Medicine> AddAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            return medicine;
        }

        public void Update(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
        }

        public void DeleteAsync(Medicine medicine)
        {
            _context.Medicines.Remove(medicine);
        }

    }
}
