using MedicineStorage.Data.Interfaces;
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
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<(IEnumerable<MedicineUsage>, int)> GetAllAsync(MedicineUsageParams parameters)
        {
            var query = _context.MedicineUsages
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
                .AsQueryable();

            if (parameters.FromDate.HasValue)
                query = query.Where(u => u.UsageDate >= parameters.FromDate);

            if (parameters.ToDate.HasValue)
                query = query.Where(u => u.UsageDate <= parameters.ToDate);

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "date" => parameters.IsDescending ?
                        query.OrderByDescending(u => u.UsageDate) :
                        query.OrderBy(u => u.UsageDate),
                    "quantity" => parameters.IsDescending ?
                        query.OrderByDescending(u => u.Quantity) :
                        query.OrderBy(u => u.Quantity),
                    _ => query.OrderByDescending(u => u.UsageDate)
                };
            }

            query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                         .Take(parameters.PageSize);

            return (await query.ToListAsync(), totalCount);
        }

        public async Task<List<MedicineUsage>> GetUsagesByMedicineIdAsync(int medicineId)
        {
            return await _context.MedicineUsages
                .Where(u => u.MedicineId == medicineId)
                .Include(u => u.UsedByUser)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByUserIdAsync(int userId)
        {
            return await _context.MedicineUsages
                .Where(u => u.UsedByUserId == userId)
                .Include(u => u.Medicine)
                .ToListAsync();
        }


        public async Task<List<MedicineUsage>> GetUsagesByRequestIdAsync(int requestId)
        {
            return await _context.MedicineUsages
                .Where(u => u.MedicineRequestId == requestId)
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
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