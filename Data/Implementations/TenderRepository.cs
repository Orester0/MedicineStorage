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
                .Include(t => t.OpenedByUser)
                .Include(t => t.Items)
                .Include(t => t.Proposals)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(IEnumerable<Tender>, int)> GetAllTendersAsync(TenderParams tenderParams)
        {
            var query = _context.Tenders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tenderParams.Title))
                query = query.Where(t => t.Title.Contains(tenderParams.Title));

            if (tenderParams.PublishDateFrom.HasValue)
                query = query.Where(t => t.PublishDate >= tenderParams.PublishDateFrom);

            if (tenderParams.PublishDateTo.HasValue)
                query = query.Where(t => t.PublishDate <= tenderParams.PublishDateTo);

            if (tenderParams.Status.HasValue)
                query = query.Where(t => t.Status == tenderParams.Status);
            query = tenderParams.OrderBy switch
            {
                "title" => query.OrderBy(t => t.Title),
                "titleDesc" => query.OrderByDescending(t => t.Title),
                "publishDate" => query.OrderBy(t => t.PublishDate),
                "publishDateDesc" => query.OrderByDescending(t => t.PublishDate),
                "status" => query.OrderBy(t => t.Status),
                _ => query.OrderByDescending(t => t.PublishDate) 
            };

            //query = query.OrderBy(t => t.Id).ThenByDescending(t => t.PublishDate);


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
                .Include(t => t.OpenedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tender>> GetTendersAwardedByUserIdAsync(int userId)
        {
            return await _context.Tenders
                .Where(t => t.WinnerSelectedByUserId == userId)
                .Include(t => t.OpenedByUser)
                .ToListAsync();
        }

        public async Task<Tender> GetTenderByProposalIdAsync(int proposalId)
        {
            return await _context.Tenders
                              .Include(t => t.Proposals)
                              .ThenInclude(p => p.Items)
                              .FirstOrDefaultAsync(t => t.Proposals.Any(p => p.Id == proposalId));

        }

        public async Task<Tender> CreateTenderAsync(Tender tender)
        {
            await _context.Tenders.AddAsync(tender);
            await _context.SaveChangesAsync();
            return tender;
        }

        public async Task UpdateTenderAsync(Tender tender)
        {
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTenderAsync(int id)
        {
            var tender = await _context.Tenders.FindAsync(id);
            if (tender != null)
            {
                _context.Tenders.Remove(tender);
                await _context.SaveChangesAsync();
            }
        }

    }
}
