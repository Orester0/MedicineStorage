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
            var query = _context.Audits.AsQueryable();

            if (auditParams.FromDate.HasValue)
                query = query.Where(x => x.PlannedDate >= auditParams.FromDate);

            if (auditParams.ToDate.HasValue)
                query = query.Where(x => x.PlannedDate <= auditParams.ToDate);

            if (auditParams.Status.HasValue)
                query = query.Where(x => x.Status == auditParams.Status);

            query = auditParams.OrderBy switch
            {
                "date" => query.OrderBy(x => x.PlannedDate),
                "dateDesc" => query.OrderByDescending(x => x.PlannedDate),
                "status" => query.OrderBy(x => x.Status),
                _ => query.OrderByDescending(x => x.PlannedDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((auditParams.PageNumber - 1) * auditParams.PageSize)
                .Take(auditParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Audit?> GetAuditByIdAsync(int auditId)
        {
            return await _context.Audits.FindAsync(auditId);
        }

        public async Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId)
        {
            return await _context.AuditItems.Where(ai => ai.AuditId == auditId).ToListAsync();
        }


        public async Task<IEnumerable<Audit>> GetAuditsByPlannedUserIdAsync(int userId)
        {
            return await _context.Audits.Where(ai => ai.ExecutedByUserId == userId).ToListAsync();
        }


        public async Task<IEnumerable<Audit>> GetAuditsByExecutedUserIdAsync(int userId)
        {
            return await _context.Audits.Where(ai => ai.PlannedByUserId == userId).ToListAsync();
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