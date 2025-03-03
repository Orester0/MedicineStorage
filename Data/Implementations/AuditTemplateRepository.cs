﻿using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.TemplateModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class AuditTemplateRepository(AppDbContext _context) : ITemplateRepository<AuditTemplate>
    {


        public async Task<IEnumerable<AuditTemplate>> GetAllActiveAndDueAsync()
        {
            return await _context.Set<AuditTemplate>()
                .Where(t => t.IsActive && (t.LastExecutedDate == null ||
                    t.LastExecutedDate.Value.AddDays(t.RecurrenceInterval) <= DateTime.UtcNow))
                .ToListAsync();
        }
        public async Task<AuditTemplate> GetByIdAsync(int id)
        {
            return await _context.AuditTemplates
               .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<IEnumerable<AuditTemplate>> GetByUserIdAsync(int id)
        {
            return await _context.AuditTemplates.Where(a => a.UserId == id).ToListAsync();
        }


        public async Task<IEnumerable<AuditTemplate>> GetAllAsync()
        {
            return await _context.AuditTemplates.ToListAsync();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<AuditTemplate> AddAsync(AuditTemplate entity)
        {
            await _context.AuditTemplates.AddAsync(entity);
            return entity;
        }

        public async Task Update(AuditTemplate entity)
        {
             _context.AuditTemplates.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var auditItem = await _context.AuditTemplates.FindAsync(id);
            if (auditItem != null)
            {
                _context.AuditTemplates.Remove(auditItem);
            }
        }
    }
}
