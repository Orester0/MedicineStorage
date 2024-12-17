using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class TenderRepository(AppDbContext _context) : ITenderRepository
    {
        public async Task<Tender> GetByIdAsync(int id)
        {
            return await _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.Items)
                .Include(t => t.Proposals)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tender>> GetAllAsync()
        {
            return await _context.Tenders
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }

        public async Task<Tender> AddAsync(Tender tender)
        {
            await _context.Tenders.AddAsync(tender);
            await _context.SaveChangesAsync();
            return tender;
        }

        public async Task UpdateAsync(Tender tender)
        {
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tender = await _context.Tenders.FindAsync(id);
            if (tender != null)
            {
                _context.Tenders.Remove(tender);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Tender>> GetTendersByStatusAsync(TenderStatus status)
        {
            return await _context.Tenders
                .Where(t => t.Status == status)
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetTendersByUserAsync(int userId)
        {
            return await _context.Tenders
                .Where(t => t.CreatedByUserId == userId)
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }
    }
}
