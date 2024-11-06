using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class TenderRepository(AppDbContext _context, ILogger<TenderRepository> _logger) : ITenderRepository
    {
        public async Task<Tender?> GetByIdAsync(int id)
        {
            return await _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Medicine)
                .Include(t => t.Proposals)
                    .ThenInclude(p => p.Items)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tender>> GetAllAsync()
        {
            return await _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Medicine)
                .ToListAsync();
        }

        public async Task<List<Tender>> GetTendersByStatusAsync(TenderStatus status)
        {
            return await _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Medicine)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<List<Tender>> GetActiveTendersAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Tenders
                .Include(t => t.Items)
                    .ThenInclude(i => i.Medicine)
                .Where(t => t.Status == TenderStatus.Published &&
                           t.DeadlineDate > now)
                .ToListAsync();
        }

        public async Task<bool> CreateTenderAsync(Tender tender)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                tender.PublishDate = DateTime.UtcNow;
                tender.Status = TenderStatus.Draft;

                _context.Tenders.Add(tender);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating tender");
                return false;
            }
        }

        public async Task<bool> UpdateTenderAsync(Tender tender)
        {
            try
            {
                var existingTender = await _context.Tenders
                    .Include(t => t.Items)
                    .FirstOrDefaultAsync(t => t.Id == tender.Id);

                if (existingTender == null) return false;
                if (existingTender.Status != TenderStatus.Draft) return false;

                existingTender.Title = tender.Title;
                existingTender.Description = tender.Description;
                existingTender.DeadlineDate = tender.DeadlineDate;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tender");
                return false;
            }
        }

        public async Task<bool> DeleteTenderAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tender = await _context.Tenders
                    .Include(t => t.Items)
                    .Include(t => t.Proposals)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tender == null) return false;
                if (tender.Status != TenderStatus.Draft) return false;

                _context.Tenders.Remove(tender);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting tender");
                return false;
            }
        }

        public async Task<bool> PublishTenderAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tender = await _context.Tenders
                    .Include(t => t.Items)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tender == null) return false;
                if (tender.Status != TenderStatus.Draft) return false;
                if (!tender.Items.Any()) return false;

                tender.Status = TenderStatus.Published;
                tender.PublishDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error publishing tender");
                return false;
            }
        }

        public async Task<bool> CloseTenderAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tender = await _context.Tenders.FindAsync(id);
                if (tender == null) return false;
                if (tender.Status != TenderStatus.Published) return false;

                tender.Status = TenderStatus.Closed;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error closing tender");
                return false;
            }
        }

        public async Task<bool> CancelTenderAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tender = await _context.Tenders.FindAsync(id);
                if (tender == null) return false;
                if (tender.Status == TenderStatus.Awarded ||
                    tender.Status == TenderStatus.Cancelled)
                    return false;

                tender.Status = TenderStatus.Cancelled;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error cancelling tender");
                return false;
            }
        }

        public async Task<bool> AddTenderItemAsync(TenderItem item)
        {
            try
            {
                var tender = await _context.Tenders.FindAsync(item.TenderId);
                if (tender == null) return false;
                if (tender.Status != TenderStatus.Draft) return false;

                _context.TenderItems.Add(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tender item");
                return false;
            }
        }

        public async Task<bool> UpdateTenderItemAsync(TenderItem item)
        {
            try
            {
                var existingItem = await _context.TenderItems
                    .Include(ti => ti.Tender)
                    .FirstOrDefaultAsync(ti => ti.Id == item.Id);

                if (existingItem == null) return false;
                if (existingItem.Tender.Status != TenderStatus.Draft) return false;

                existingItem.RequiredQuantity = item.RequiredQuantity;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tender item");
                return false;
            }
        }

        public async Task<bool> RemoveTenderItemAsync(int itemId)
        {
            try
            {
                var item = await _context.TenderItems
                    .Include(ti => ti.Tender)
                    .FirstOrDefaultAsync(ti => ti.Id == itemId);

                if (item == null) return false;
                if (item.Tender.Status != TenderStatus.Draft) return false;

                _context.TenderItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing tender item");
                return false;
            }
        }

        public async Task<List<Tender>> GetTendersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Tenders
                .Include(t => t.Items)
                    .ThenInclude(i => i.Medicine)
                .Where(t => t.PublishDate >= startDate && t.PublishDate <= endDate)
                .ToListAsync();
        }
    }
}
