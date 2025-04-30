using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineSupplyRepository(AppDbContext _context)
    : GenericRepository<MedicineSupply>(_context), IMedicineSupplyRepository
    {
        public async Task<List<MedicineSupply>> GetSuppliesByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate)
        {
            return await _context.MedicineSupplies
                .Where(s => s.MedicineId == medicineId)
                .Where(s => s.TransactionDate >= startDate && s.TransactionDate <= endDate)
                .Include(s => s.Medicine)
                .ToListAsync();
        }

        public async Task<Dictionary<int, List<MedicineSupply>>> GetAllMedicineSuppliesInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var supplies = await _context.MedicineSupplies
                .Where(s => s.TransactionDate >= startDate && s.TransactionDate <= endDate)
                .Include(s => s.Medicine)
                .ToListAsync();

            return supplies.GroupBy(s => s.MedicineId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
        public async Task<(IEnumerable<MedicineSupply>, int)> GetByParamsAsync(MedicineSupplyParams parameters)
        {
            var query = _context.MedicineSupplies
                .Include(ms => ms.Medicine)
                .Include(ms => ms.CreatedByUser)
                .Include(ms => ms.Tender)
                .AsQueryable();

            if (parameters.MedicineId.HasValue)
                query = query.Where(ms => ms.MedicineId == parameters.MedicineId);

            if (parameters.TenderId.HasValue)
                query = query.Where(ms => ms.TenderId == parameters.TenderId);

            if (parameters.CreatedByUserId.HasValue)
                query = query.Where(ms => ms.CreatedByUserId == parameters.CreatedByUserId);

            if (parameters.StartDate.HasValue)
                query = query.Where(ms => ms.TransactionDate >= parameters.StartDate);

            if (parameters.EndDate.HasValue)
                query = query.Where(ms => ms.TransactionDate <= parameters.EndDate);

            query = parameters.SortBy?.ToLower() switch
            {
                "id" => parameters.IsDescending ? query.OrderByDescending(ms => ms.Id) : query.OrderBy(ms => ms.Id),
                "medicine" => parameters.IsDescending ? query.OrderByDescending(r => r.Medicine.Name) : query.OrderBy(r => r.Medicine.Name),
                "tender" => parameters.IsDescending ? query.OrderByDescending(r => r.TenderId) : query.OrderBy(r => r.TenderId),
                "createdbyuser" => parameters.IsDescending ? query.OrderByDescending(r => r.CreatedByUserId) : query.OrderBy(r => r.CreatedByUserId),
                "transactiondate" => parameters.IsDescending ? query.OrderByDescending(ms => ms.TransactionDate) : query.OrderBy(ms => ms.TransactionDate),
                _ => query.OrderBy(ms => ms.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public override async Task<List<MedicineSupply>> GetAllAsync()
        {
            return await _context.Set<MedicineSupply>()
                .Include(it => it.Medicine)
                .ToListAsync();
        }

        public override async Task<MedicineSupply?> GetByIdAsync(int id)
        {
            return await _context.MedicineSupplies
                .Include(it => it.Medicine)
                .FirstOrDefaultAsync(it => it.Id == id);
        }

    }
}