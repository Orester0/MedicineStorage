using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class AuditRepository(AppDbContext _context) : GenericRepository<Audit>(_context), IAuditRepository
    {
        public override async Task<Audit?> GetByIdAsync(int id)
        {
            return await _context.Audits
                .Include(a => a.PlannedByUser)
                .Include(a => a.ClosedByUser)
                .Include(a => a.Notes)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.Medicine)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.CheckedByUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<(IEnumerable<Audit>, int)> GetByParams(AuditParams auditParams)
        {
            var query = _context.Audits
                .Include(a => a.PlannedByUser)
                .Include(a => a.ClosedByUser)
                .Include(a => a.Notes)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.Medicine)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.CheckedByUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(auditParams.Title))
                query = query.Where(x => x.Title.Contains(auditParams.Title));

            if (auditParams.FromPlannedDate.HasValue)
                query = query.Where(x => x.PlannedDate >= auditParams.FromPlannedDate);

            if (auditParams.ToPlannedDate.HasValue)
                query = query.Where(x => x.PlannedDate <= auditParams.ToPlannedDate);

            if (auditParams.Statuses != null && auditParams.Statuses.Any())
            {
                query = query.Where(x => auditParams.Statuses.Contains(x.Status));
            }

            if (auditParams.PlannedByUserId.HasValue)
                query = query.Where(x => x.PlannedByUserId == auditParams.PlannedByUserId);

            if (auditParams.ClosedByUserId.HasValue)
                query = query.Where(x => x.ClosedByUserId == auditParams.ClosedByUserId);

            if (auditParams.ExecutedByUserId.HasValue)
                query = query.Where(x => x.ClosedByUserId == auditParams.ExecutedByUserId);

            query = auditParams.SortBy?.ToLower() switch
            {
                "id" => auditParams.IsDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                "title" => auditParams.IsDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                "planneddate" => auditParams.IsDescending ? query.OrderByDescending(x => x.PlannedDate) : query.OrderBy(x => x.PlannedDate),
                "startdate" => auditParams.IsDescending ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate),
                "enddate" => auditParams.IsDescending ? query.OrderByDescending(x => x.EndDate) : query.OrderBy(x => x.EndDate),
                "status" => auditParams.IsDescending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
                _ => auditParams.IsDescending? query.OrderByDescending(x => x.Id): query.OrderBy(x => x.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((auditParams.PageNumber - 1) * auditParams.PageSize)
                .Take(auditParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }




        public async Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId)
        {
            return await _context.AuditItems
                .Where(ai => ai.AuditId == auditId)
                .Include(ai => ai.Medicine)
                .Include(ai => ai.CheckedByUser)
                .ToListAsync();
        }


        public async Task<IEnumerable<Audit>> GetAuditsByPlannedUserIdAsync(int userId)
        {
            return await _context.Audits
                .Where(ai => ai.ClosedByUserId == userId)
                .Include(a => a.PlannedByUser)
                .Include(a => a.ClosedByUser)
                .Include(a => a.Notes)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.Medicine)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.CheckedByUser)
                .ToListAsync();
        }


        public async Task<IEnumerable<Audit>> GetAuditsByExecutedUserIdAsync(int userId)
        {
            return await _context.Audits
                .Where(ai => ai.PlannedByUserId == userId)
                .Include(a => a.PlannedByUser)
                .Include(a => a.ClosedByUser)
                .Include(a => a.Notes)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.Medicine)
                .Include(a => a.AuditItems)
                    .ThenInclude(ai => ai.CheckedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate)
        {
            return await _context.Audits
                .Include(a => a.AuditItems)
                .Where(a => a.AuditItems.Any(ai => ai.MedicineId == medicineId))
                .Where(a => (a.PlannedDate >= startDate && a.PlannedDate <= endDate) ||
                    (a.StartDate.HasValue && a.StartDate.Value >= startDate && a.StartDate.Value <= endDate) ||
                    (a.EndDate.HasValue && a.EndDate.Value >= startDate && a.EndDate.Value <= endDate))
                .ToListAsync();
        }


        public async Task<IEnumerable<AuditItem>> CreateAuditItemsAsync(IEnumerable<AuditItem> auditItems)
        {
            await _context.AuditItems.AddRangeAsync(auditItems);
            return auditItems;
        }

        public void UpdateAuditItem(AuditItem auditItem)
        {
            _context.AuditItems.Update(auditItem);

        }
    }
}