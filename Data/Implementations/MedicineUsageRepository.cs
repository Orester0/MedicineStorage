using MedicineStorage.Data.Interfaces;
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

        public async Task<List<MedicineUsage>> GetAllAsync()
        {
            return await _context.MedicineUsages
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByMedicineAsync(int medicineId)
        {
            return await _context.MedicineUsages
                .Where(u => u.MedicineId == medicineId)
                .Include(u => u.UsedByUser)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByUserAsync(int userId)
        {
            return await _context.MedicineUsages
                .Where(u => u.UsedByUserId == userId)
                .Include(u => u.Medicine)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.MedicineUsages
                .Where(u => u.UsageDate >= startDate && u.UsageDate <= endDate)
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
                .ToListAsync();
        }

        public async Task<MedicineUsage> AddUsageAsync(MedicineUsage usage)
        {
            usage.UsageDate = DateTime.UtcNow;
            _context.MedicineUsages.Add(usage);
            await _context.SaveChangesAsync();
            return usage;
        }

        public async Task<MedicineUsage> UpdateUsageAsync(MedicineUsage usage)
        {
            _context.MedicineUsages.Update(usage);
            await _context.SaveChangesAsync();

            // Reload the entity to get the updated version with includes
            return await GetByIdAsync(usage.Id) ?? usage;
        }

        public async Task<bool> DeleteUsageAsync(int id)
        {
            var usage = await _context.MedicineUsages.FindAsync(id);
            if (usage != null)
            {
                _context.MedicineUsages.Remove(usage);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<decimal> GetTotalUsageQuantityAsync(int medicineId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.MedicineUsages
                .Where(u => u.MedicineId == medicineId);

            if (startDate.HasValue)
                query = query.Where(u => u.UsageDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(u => u.UsageDate <= endDate.Value);

            return await query.SumAsync(u => u.Quantity);
        }


        public async Task<List<MedicineUsage>> GetUsagesByRequestIdAsync(int requestId)
        {
            return await _context.MedicineUsages
                .Where(u => u.MedicineRequestId == requestId)
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
                .ToListAsync();
        }


    }
}