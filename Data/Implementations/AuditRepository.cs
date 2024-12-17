using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class AuditRepository(AppDbContext _context) : IAuditRepository
    {
        public async Task<IEnumerable<Audit>> GetAllAuditsAsync()
        {
            return await _context.Audits.ToListAsync();
        }

        public async Task<Audit?> GetAuditByIdAsync(int auditId)
        {
            return await _context.Audits.FindAsync(auditId);
        }

        public async Task<IEnumerable<AuditItem>> GetAuditItemsByAuditIdAsync(int auditId)
        {
            return await _context.AuditItems.Where(ai => ai.AuditId == auditId).ToListAsync();
        }

        public async Task AddAsync(Audit audit)
        {
            _context.Audits.Add(audit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Audit audit)
        {
            _context.Audits.Update(audit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int auditId)
        {
            var audit = await _context.Audits.FindAsync(auditId);
            if (audit != null)
            {
                _context.Audits.Remove(audit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAuditItemAsync(AuditItem auditItem)
        {
            _context.AuditItems.Add(auditItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuditItemAsync(AuditItem auditItem)
        {
            _context.AuditItems.Update(auditItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuditItemAsync(int auditItemId)
        {
            var auditItem = await _context.AuditItems.FindAsync(auditItemId);
            if (auditItem != null)
            {
                _context.AuditItems.Remove(auditItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}