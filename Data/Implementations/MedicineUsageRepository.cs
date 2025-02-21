using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineUsageRepository(AppDbContext _context)
    : GenericRepository<MedicineUsage>(_context), IMedicineUsageRepository
    {
        public override async Task<MedicineUsage?> GetByIdAsync(int id)
        {
            return await _context.MedicineUsages
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<(IEnumerable<MedicineUsage>, int)> GetByParams(MedicineUsageParams parameters)
        {
            var query = _context.MedicineUsages
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .AsQueryable();

            if (parameters.FromDate.HasValue)
                query = query.Where(u => u.UsageDate >= parameters.FromDate);

            if (parameters.ToDate.HasValue)
                query = query.Where(u => u.UsageDate <= parameters.ToDate);

            if (parameters.MedicineId.HasValue)
                query = query.Where(u => u.MedicineId == parameters.MedicineId);

            query = parameters.SortBy?.ToLower() switch
            {
                "id" => parameters.IsDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                "medicine" => parameters.IsDescending ? query.OrderByDescending(r => r.Medicine.Name) : query.OrderBy(r => r.Medicine.Name),
                "usagedate" => parameters.IsDescending ? query.OrderByDescending(u => u.UsageDate) : query.OrderBy(u => u.UsageDate),
                "usedbyuser" => parameters.IsDescending ? query.OrderByDescending(u => u.UsedByUserId) : query.OrderBy(u => u.UsedByUserId),
                _ => parameters.IsDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<List<MedicineUsage>> GetUsagesByUserIdAsync(int userId)
        {
            return await _context.MedicineUsages
                .Where(u => u.UsedByUserId == userId)
                .Include(u => u.UsedByUser)
                .Include(u => u.Medicine)
                .ToListAsync();
        }

       

    }
}