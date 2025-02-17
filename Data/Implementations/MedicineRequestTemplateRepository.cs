using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TemplateModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRequestTemplateRepository(AppDbContext _context) : ITemplateRepository<MedicineRequestTemplate>
    {

        public async Task<IEnumerable<MedicineRequestTemplate>> GetAllActiveAndDueAsync()
        {
            return await _context.Set<MedicineRequestTemplate>()
                .Where(t => t.IsActive && (t.LastExecutedDate == null ||
                    t.LastExecutedDate.Value.AddDays(t.RecurrenceInterval) <= DateTime.UtcNow))
                .ToListAsync();
        }

        public async Task<MedicineRequestTemplate> GetByIdAsync(int id)
        {
            return await _context.MedicineRequestTemplates
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<MedicineRequestTemplate>> GetByUserIdAsync(int id)
        {
            return await _context.MedicineRequestTemplates.Where(a => a.UserId == id).ToListAsync();
        }

        public async Task<IEnumerable<MedicineRequestTemplate>> GetAllAsync()
        {
            return await _context.MedicineRequestTemplates.ToListAsync();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        public async Task<MedicineRequestTemplate> AddAsync(MedicineRequestTemplate entity)
        {
            await _context.MedicineRequestTemplates.AddAsync(entity);
            return entity;
        }

        public async Task Update(MedicineRequestTemplate entity)
        {
            _context.MedicineRequestTemplates.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var requestItem = await _context.MedicineRequestTemplates.FindAsync(id);
            if (requestItem != null)
            {
                _context.MedicineRequestTemplates.Remove(requestItem);
            }
        }
    }
}
