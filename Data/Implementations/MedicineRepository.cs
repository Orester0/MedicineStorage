using MedicineStorage.Data.Interfaces;

using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRepository(AppDbContext _context)
    : GenericRepository<Medicine>(_context), IMedicineRepository

    {

        public override async Task<List<Medicine>> GetAllAsync()
        {
            return await _context.Medicines.OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetAllRequiringAuditAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Medicines
                .Where(m => m.LastAuditDate != null &&
                           m.LastAuditDate.Value.AddDays(m.AuditFrequencyDays) <= currentDate)
                .ToListAsync();
        }


        public async Task<List<Medicine>> GetByIdsAsync(IEnumerable<int> medicineIds)
        {
            return await _context.Medicines
                .Where(m => medicineIds.Contains(m.Id))
                .ToListAsync();
        }

        public async Task<(IEnumerable<Medicine>, int)> GetByParams(MedicineParams parameters)
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
            
            query = parameters.SortBy?.ToLower() switch
            {
                "id" => parameters.IsDescending ? query.OrderByDescending(m => m.Id) : query.OrderBy(m => m.Id),
                "name" => parameters.IsDescending ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
                "category" => parameters.IsDescending ? query.OrderByDescending(m => m.Category) : query.OrderBy(m => m.Category),
                "stock" => parameters.IsDescending ? query.OrderByDescending(m => m.Stock) : query.OrderBy(m => m.Stock),
                _ => query.OrderBy(m => m.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }


    }
}
