using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
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
                .Include(t => t.OpenedByUser)
                .Include(t => t.ClosedByUser)
                .Include(t => t.WinnerSelectedByUser)
                .Include(t => t.TenderItems)
                .Include(t => t.TenderProposals)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(IEnumerable<Tender>, int)> GetAllTendersAsync(TenderParams tenderParams)
        {
            var query = _context.Tenders
                .Include(t => t.CreatedByUser)
                .Include(t => t.OpenedByUser)
                .Include(t => t.ClosedByUser)
                .Include(t => t.WinnerSelectedByUser)
                .Include(t => t.TenderItems)
                .Include(t => t.TenderProposals)
                .AsQueryable();

            // Фільтрація
            if (!string.IsNullOrWhiteSpace(tenderParams.Title))
                query = query.Where(t => t.Title.Contains(tenderParams.Title));

            if (tenderParams.PublishDateFrom.HasValue)
                query = query.Where(t => t.PublishDate >= tenderParams.PublishDateFrom);

            if (tenderParams.PublishDateTo.HasValue)
                query = query.Where(t => t.PublishDate <= tenderParams.PublishDateTo);

            if (tenderParams.ClosingDateFrom.HasValue)
                query = query.Where(t => t.ClosingDate >= tenderParams.ClosingDateFrom);

            if (tenderParams.ClosingDateTo.HasValue)
                query = query.Where(t => t.ClosingDate <= tenderParams.ClosingDateTo);

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

            // Сортування
            query = tenderParams.SortBy?.ToLower() switch
            {
                "title" => tenderParams.IsDescending
                    ? query.OrderByDescending(t => t.Title)
                    : query.OrderBy(t => t.Title),
                "publishdate" => tenderParams.IsDescending
                    ? query.OrderByDescending(t => t.PublishDate)
                    : query.OrderBy(t => t.PublishDate),
                "closingdate" => tenderParams.IsDescending
                    ? query.OrderByDescending(t => t.ClosingDate)
                    : query.OrderBy(t => t.ClosingDate),
                "deadlinedate" => tenderParams.IsDescending
                    ? query.OrderByDescending(t => t.DeadlineDate)
                    : query.OrderBy(t => t.DeadlineDate),
                "status" => tenderParams.IsDescending
                    ? query.OrderByDescending(t => t.Status)
                    : query.OrderBy(t => t.Status),
                _ => tenderParams.IsDescending
                    ? query.OrderByDescending(t => t.PublishDate)
                    : query.OrderBy(t => t.PublishDate)
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

        public async Task<Tender> CreateTenderAsync(Tender tender)
        {
            await _context.Tenders.AddAsync(tender);
            return tender;
        }

        public void UpdateTender(Tender tender)
        {
            _context.Tenders.Update(tender);
        }

        public async Task DeleteTenderAsync(int id)
        {
            var tender = await _context.Tenders.FindAsync(id);
            if (tender != null)
            {
                _context.Tenders.Remove(tender);
            }
        }

    }
}
