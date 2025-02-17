using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.AuditModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class AuditRepository(AppDbContext _context) : IAuditRepository
    {
        public async Task<(IEnumerable<Audit>, int)> GetAllAuditsAsync(AuditParams auditParams)
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

            if (auditParams.Status.HasValue)
                query = query.Where(x => x.Status == auditParams.Status);

            if (auditParams.PlannedByUserId.HasValue)
                query = query.Where(x => x.PlannedByUserId == auditParams.PlannedByUserId);

            if (auditParams.ClosedByUserId.HasValue)
                query = query.Where(x => x.ClosedByUserId == auditParams.ClosedByUserId);

            if (auditParams.ExecutedByUserId.HasValue)
                query = query.Where(x => x.ClosedByUserId == auditParams.ExecutedByUserId);

            query = auditParams.SortBy?.ToLower() switch
            {
                "title" => auditParams.IsDescending
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),
                "planneddate" => auditParams.IsDescending
                    ? query.OrderByDescending(x => x.PlannedDate)
                    : query.OrderBy(x => x.PlannedDate),
                "startdate" => auditParams.IsDescending
                    ? query.OrderByDescending(x => x.StartDate)
                    : query.OrderBy(x => x.StartDate),
                "enddate" => auditParams.IsDescending
                    ? query.OrderByDescending(x => x.EndDate)
                    : query.OrderBy(x => x.EndDate),
                "status" => auditParams.IsDescending
                    ? query.OrderByDescending(x => x.Status)
                    : query.OrderBy(x => x.Status),
                _ => auditParams.IsDescending
                    ? query.OrderByDescending(x => x.PlannedDate)
                    : query.OrderBy(x => x.PlannedDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((auditParams.PageNumber - 1) * auditParams.PageSize)
                .Take(auditParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }



        public async Task<Audit?> GetByIdAsync(int id)
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

        public async Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId)
        {
            return await _context.AuditItems
                .Where(ai => ai.AuditId == auditId)
                .Include(ai => ai.Medicine)
                .Include (ai => ai.CheckedByUser)
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<Audit> CreateAuditAsync(Audit audit)
        {
            await _context.Audits.AddAsync(audit);
            return audit;
        }

        public void UpdateAudit(Audit audit)
        {
            _context.Audits.Update(audit);
             
        }

        public async Task DeleteAuditAsync(int auditId)
        {
            var audit = await _context.Audits.FindAsync(auditId);
            if (audit != null)
            {
                _context.Audits.Remove(audit);
            }
        }

        public async Task<AuditItem> CreateAuditItemAsync(AuditItem auditItem)
        {
            await _context.AuditItems.AddAsync(auditItem);
            return auditItem;
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

        public async Task DeleteAuditItemAsync(int auditItemId)
        {
            var auditItem = await _context.AuditItems.FindAsync(auditItemId);
            if (auditItem != null)
            {
                _context.AuditItems.Remove(auditItem);
                 
            }
        }
    }
}