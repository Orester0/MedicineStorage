﻿using MedicineStorage.Data.Interfaces;
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


        public async Task<IEnumerable<Tender>> GetByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate)
        {
            return await _context.Tenders
                .Include(t => t.TenderItems)
                .Where(t => t.TenderItems.Any(ti => ti.MedicineId == medicineId))
                .Where(t => (t.DeadlineDate >= startDate && t.DeadlineDate <= endDate) ||
                    (t.PublishDate >= startDate && t.PublishDate <= endDate) ||
                    (t.ClosingDate.HasValue && t.ClosingDate.Value >= startDate && t.ClosingDate.Value <= endDate))
                .ToListAsync();
        }

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

            if (tenderParams.Statuses != null && tenderParams.Statuses.Any())
            {
                query = query.Where(t => tenderParams.Statuses.Contains(t.Status));
            }


            if (tenderParams.CreatedByUserId.HasValue)
                query = query.Where(t => t.CreatedByUserId == tenderParams.CreatedByUserId);

            if (tenderParams.OpenedByUserId.HasValue)
                query = query.Where(t => t.OpenedByUserId == tenderParams.OpenedByUserId);

            if (tenderParams.ClosedByUserId.HasValue)
                query = query.Where(t => t.ClosedByUserId == tenderParams.ClosedByUserId);

            if (tenderParams.WinnerSelectedByUserId.HasValue)
                query = query.Where(t => t.WinnerSelectedByUserId == tenderParams.WinnerSelectedByUserId);

            if (tenderParams.MedicineIds != null && tenderParams.MedicineIds.Any())
            {
                query = query.Where(t => t.TenderItems.Any(ti => tenderParams.MedicineIds.Contains(ti.MedicineId)));
            }


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

        public async Task<IEnumerable<Tender>> GetPublishedTendersAsync()
        {
            return await _context.Tenders
                .Where(t => t.Status == TenderStatus.Published)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetRelevantTendersAsync()
        {
            var validStatuses = new List<TenderStatus>
            {
                TenderStatus.Created,
                TenderStatus.Published,
                TenderStatus.Closed,
                TenderStatus.Awarded
            };

            return await _context.Tenders
                .Where(t => validStatuses.Contains(t.Status))
                .Include(t => t.TenderItems)
                .ToListAsync();
        }

    }
}
