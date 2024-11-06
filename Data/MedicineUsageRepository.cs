using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data
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
                .Include(u => u.UsedByUser)
                .Where(u => u.MedicineId == medicineId)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByUserAsync(int userId)
        {
            return await _context.MedicineUsages
                .Include(u => u.Medicine)
                .Where(u => u.UsedByUserId == userId)
                .ToListAsync();
        }

        public async Task<List<MedicineUsage>> GetUsagesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.MedicineUsages
                .Include(u => u.Medicine)
                .Include(u => u.UsedByUser)
                .Where(u => u.UsageDate >= startDate && u.UsageDate <= endDate)
                .ToListAsync();
        }

        public async Task<bool> RecordUsageAsync(MedicineUsage usage)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var medicine = await _context.Medicines
                    .FirstOrDefaultAsync(m => m.Id == usage.MedicineId);

                if (medicine == null || medicine.Stock < usage.Quantity)
                    return false;

                medicine.Stock -= usage.Quantity;

                usage.UsageDate = DateTime.UtcNow;
                _context.MedicineUsages.Add(usage);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task<bool> UpdateUsageAsync(MedicineUsage usage)
        {
            try
            {
                _context.MedicineUsages.Update(usage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUsageAsync(int id)
        {
            var usage = await _context.MedicineUsages.FindAsync(id);
            if (usage == null) return false;

            try
            {
                _context.MedicineUsages.Remove(usage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal> GetTotalUsageForMedicineAsync(int medicineId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.MedicineUsages
                .Where(u => u.MedicineId == medicineId);

            if (startDate.HasValue)
                query = query.Where(u => u.UsageDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(u => u.UsageDate <= endDate.Value);

            return await query.SumAsync(u => u.Quantity);
        }
    }
}
