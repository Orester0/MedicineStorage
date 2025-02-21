using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Implementations
{
    public class TenderRepository(AppDbContext _context)
    : GenericRepository<Tender>(_context), ITenderRepository
    {

        public override async Task<List<Tender>> GetAllAsync()
        {
            return await _context.Tenders.OrderBy(m => m.Title).ToListAsync();
        }

        public override async Task<Tender?> GetByIdAsync(int id)
        {
            return await _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.OpenedByUser)
                .Include(t => t.ClosedByUser)
                .Include(t => t.WinnerSelectedByUser)
                .Include(t => t.TenderItems)
                .ThenInclude(ti => ti.Medicine)
                .Include(t => t.TenderProposals)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(IEnumerable<Tender>, int)> GetByParams(TenderParams tenderParams)
        {
            var query = _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.OpenedByUser)
                .Include(t => t.ClosedByUser)
                .Include(t => t.WinnerSelectedByUser)
                .Include(t => t.TenderItems)
                    .ThenInclude(ti => ti.Medicine)
                .Include(t => t.TenderProposals)
                .AsQueryable();

            // Фільтрація
            if (!string.IsNullOrWhiteSpace(tenderParams.Title))
                query = query.Where(t => t.Title.Contains(tenderParams.Title));

            if (tenderParams.DeadlineDateFrom.HasValue)
                query = query.Where(t => t.DeadlineDate >= tenderParams.DeadlineDateFrom);

            if (tenderParams.DeadlineDateTo.HasValue)
                query = query.Where(t => t.DeadlineDate <= tenderParams.DeadlineDateTo);

            if (tenderParams.Status.HasValue)
                query = query.Where(t => t.Status == tenderParams.Status);

            if (tenderParams.CreatedByUserId.HasValue)
                query = query.Where(t => t.CreatedByUserId == tenderParams.CreatedByUserId);

            if (tenderParams.OpenedByUserId.HasValue)
                query = query.Where(t => t.OpenedByUserId == tenderParams.OpenedByUserId);

            if (tenderParams.ClosedByUserId.HasValue)
                query = query.Where(t => t.ClosedByUserId == tenderParams.ClosedByUserId);

            if (tenderParams.WinnerSelectedByUserId.HasValue)
                query = query.Where(t => t.WinnerSelectedByUserId == tenderParams.WinnerSelectedByUserId);

            if (tenderParams.MedicineId.HasValue)
                query = query.Where(t => t.TenderItems.Any(ti => ti.MedicineId == tenderParams.MedicineId));

            // Сортування
            query = tenderParams.SortBy?.ToLower() switch
            {
                "id" => tenderParams.IsDescending? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id),
                "title" => tenderParams.IsDescending? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
                "status" => tenderParams.IsDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
                "deadlinedate" => tenderParams.IsDescending? query.OrderByDescending(t => t.DeadlineDate) : query.OrderBy(t => t.DeadlineDate),
                _ => tenderParams.IsDescending? query.OrderByDescending(t => t.PublishDate) : query.OrderBy(t => t.PublishDate)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((tenderParams.PageNumber - 1) * tenderParams.PageSize)
                .Take(tenderParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<Tender>> GetTendersCreatedByUserIdAsync(int userId)
        {
            return await _context.Tenders
                .Where(t => t.CreatedByUserId == userId)
                .Include(t => t.CreatedByUser)
                .Include(t => t.OpenedByUser)
                .Include(t => t.ClosedByUser)
                .Include(t => t.WinnerSelectedByUser)
                .Include(t => t.TenderItems)
                .Include(t => t.TenderProposals)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetTendersAwardedByUserIdAsync(int userId)
        {
            return await _context.Tenders
                .Where(t => t.WinnerSelectedByUserId == userId)
                .Include(t => t.CreatedByUser)
                .Include(t => t.OpenedByUser)
                .Include(t => t.ClosedByUser)
                .Include(t => t.WinnerSelectedByUser)
                .Include(t => t.TenderItems)
                .Include(t => t.TenderProposals)
                .ToListAsync();
        }

        public async Task<Tender> GetTenderByProposalIdAsync(int proposalId)
        {
            return await _context.Tenders
                              .Include(t => t.TenderProposals)
                              .Include(t => t.CreatedByUser)
                            .Include(t => t.OpenedByUser)
                            .Include(t => t.ClosedByUser)
                            .Include(t => t.WinnerSelectedByUser)
                            .Include(t => t.TenderItems)
                              .FirstOrDefaultAsync(t => t.TenderProposals.Any(p => p.Id == proposalId));

        }

    }
}
