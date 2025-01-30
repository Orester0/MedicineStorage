using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TemplateModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderTemplateRepository(AppDbContext _context) : INotificationTemplateRepository<TenderTemplate>
    {


        public async Task<TenderTemplate> GetByIdAsync(int id)
        {
            return await _context.TenderTemplates.FindAsync(id);
        }


        public async Task<IEnumerable<TenderTemplate>> GetByUserIdAsync(int id)
        {
            return await _context.TenderTemplates.Where(a => a.UserId == id).ToListAsync();
        }

        public async Task<IEnumerable<TenderTemplate>> GetAllAsync()
        {
            return await _context.TenderTemplates.ToListAsync();
        }

        public async Task<TenderTemplate> AddAsync(TenderTemplate entity)
        {
            await _context.TenderTemplates.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(TenderTemplate entity)
        {
            _context.TenderTemplates.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var tenderitem = await _context.TenderTemplates.FindAsync(id);
            if (tenderitem != null)
            {
                _context.TenderTemplates.Remove(tenderitem);
            }
        }
    }
}
