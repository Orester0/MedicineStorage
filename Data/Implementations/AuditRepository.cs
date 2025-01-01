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
                .Include(a => a.ExecutedByUser)
                .Include(a => a.AuditItems)
                .AsQueryable();

            // Фільтрація
            if (auditParams.FromDate.HasValue)
                query = query.Where(x => x.PlannedDate >= auditParams.FromDate);

            if (auditParams.ToDate.HasValue)
                query = query.Where(x => x.PlannedDate <= auditParams.ToDate);

            if (auditParams.Status.HasValue)
                query = query.Where(x => x.Status == auditParams.Status);

            if (auditParams.PlannedByUserId.HasValue)
                query = query.Where(x => x.PlannedByUserId == auditParams.PlannedByUserId);

            if (auditParams.ExecutedByUserId.HasValue)
                query = query.Where(x => x.ExecutedByUserId == auditParams.ExecutedByUserId);

            if (!string.IsNullOrWhiteSpace(auditParams.Notes))
                query = query.Where(x => x.Notes != null && x.Notes.Contains(auditParams.Notes));

            // Сортування
            query = auditParams.SortBy?.ToLower() switch
            {
                "date" => auditParams.IsDescending
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

            // Пагінація
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((auditParams.PageNumber - 1) * auditParams.PageSize)
                .Take(auditParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<Audit?> GetAuditByIdAsync(int id)
        {
            return await _context.Audits
                .Include(a => a.PlannedByUser)
                .Include(a => a.ExecutedByUser)
                .Include(a => a.AuditItems)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId)
        {
            return await _context.AuditItems
                .Where(ai => ai.AuditId == auditId)
                .Include(ai => ai.Medicine)
                .ToListAsync();
        }


        public async Task<IEnumerable<Audit>> GetAuditsByPlannedUserIdAsync(int userId)
        {
            return await _context.Audits
                .Where(ai => ai.ExecutedByUserId == userId)
                .Include(a => a.PlannedByUser)
                .Include(a => a.ExecutedByUser)
                .Include(a => a.AuditItems)
                .ToListAsync();
        }


        public async Task<IEnumerable<Audit>> GetAuditsByExecutedUserIdAsync(int userId)
        {
            return await _context.Audits
                .Where(ai => ai.PlannedByUserId == userId)
                .Include(a => a.PlannedByUser)
                .Include(a => a.ExecutedByUser)
                .Include(a => a.AuditItems)
                .ToListAsync();
        }

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