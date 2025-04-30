using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRequestRepository(AppDbContext _context)
    : GenericRepository<MedicineRequest>(_context), IMedicineRequestRepository
    {
        public async Task<(IEnumerable<MedicineRequestAnalysisDto>, int)> GetRequestAnalysisByParamsAsync(MedicineRequestAnalysisParams parameters)
        {
            var query = _context.MedicineRequests
                .Include(r => r.Medicine)
                .AsQueryable();

            query = query.Where(r => r.RequestDate >= parameters.StartDate);
            query = query.Where(r => r.RequestDate <= parameters.EndDate);

            if (parameters.MedicineId.HasValue)
                query = query.Where(r => r.MedicineId == parameters.MedicineId);

            if (parameters.Statuses != null && parameters.Statuses.Any())
                query = query.Where(r => parameters.Statuses.Contains(r.Status));

            var groupedQuery = query
                .GroupBy(r => r.MedicineId)
                .Select(g => new MedicineRequestAnalysisDto
                {
                    MedicineName = g.First().Medicine.Name,
                    TotalRequests = g.Count(),
                    TotalQuantity = (int)g.Sum(r => r.Quantity),
                    UniqueRequesters = g.Select(r => r.RequestedByUserId).Distinct().Count(),
                    ApprovedCount = g.Count(r => r.Status == RequestStatus.Approved),
                    RejectedCount = g.Count(r => r.Status == RequestStatus.Rejected),
                    PendingCount = g.Count(r => r.Status == RequestStatus.Pending || r.Status == RequestStatus.PedingWithSpecial)
                });

            var totalCount = await groupedQuery.CountAsync();

            groupedQuery = parameters.SortBy?.ToLower() switch
            {
                "medicinename" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.MedicineName)
                    : groupedQuery.OrderBy(x => x.MedicineName),
                "totalrequests" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.TotalRequests)
                    : groupedQuery.OrderBy(x => x.TotalRequests),
                "totalquantity" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.TotalQuantity)
                    : groupedQuery.OrderBy(x => x.TotalQuantity),
                "uniquerequesters" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.UniqueRequesters)
                    : groupedQuery.OrderBy(x => x.UniqueRequesters),
                "approvedcount" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.ApprovedCount)
                    : groupedQuery.OrderBy(x => x.ApprovedCount),
                "rejectedcount" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.RejectedCount)
                    : groupedQuery.OrderBy(x => x.RejectedCount),
                "pendingcount" => parameters.IsDescending
                    ? groupedQuery.OrderByDescending(x => x.PendingCount)
                    : groupedQuery.OrderBy(x => x.PendingCount),
                _ => groupedQuery.OrderBy(x => x.MedicineName)
            };

            var items = await groupedQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }





        public async Task<Dictionary<int, List<MedicineRequest>>> GetAllMedicineRequestsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var requests = await _context.MedicineRequests
                .Where(r => (r.RequestDate >= startDate && r.RequestDate <= endDate) ||
                            (r.RequiredByDate >= startDate && r.RequiredByDate <= endDate) ||
                            (r.ApprovalDate.HasValue && r.ApprovalDate.Value >= startDate && r.ApprovalDate.Value <= endDate))
                .Include(r => r.Medicine)
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .ToListAsync();

            return requests.GroupBy(r => r.MedicineId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
        public async Task<IEnumerable<MedicineRequest>> GetByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate)
        {
            return await _context.MedicineRequests
                .Where(r => r.MedicineId == medicineId)
                .Where(r => (r.RequestDate >= startDate && r.RequestDate <= endDate) ||
                    (r.RequiredByDate >= startDate && r.RequiredByDate <= endDate) ||
                    (r.ApprovalDate.HasValue && r.ApprovalDate.Value >= startDate && r.ApprovalDate.Value <= endDate))
                .ToListAsync();
        }
        public override async Task<MedicineRequest?> GetByIdAsync(int id)
        {
            return await _context.MedicineRequests
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Medicine)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        
        public async Task<(IEnumerable<MedicineRequest>, int)> GetByParams(MedicineRequestParams parameters)
        {
            var query = _context.MedicineRequests
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Medicine)
                .AsQueryable();

            if (parameters.FromDate.HasValue)
                query = query.Where(r => r.RequiredByDate >= parameters.FromDate);

            if (parameters.ToDate.HasValue)
                query = query.Where(r => r.RequiredByDate <= parameters.ToDate);

            if (parameters.Statuses != null && parameters.Statuses.Any())
                query = query.Where(r => parameters.Statuses.Contains(r.Status));


            if (parameters.RequestedByUserId.HasValue)
                query = query.Where(r => r.RequestedByUserId == parameters.RequestedByUserId);

            if (parameters.ApprovedByUserId.HasValue)
                query = query.Where(r => r.ApprovedByUserId == parameters.ApprovedByUserId);

            if (parameters.MedicineId.HasValue)
                query = query.Where(r => r.MedicineId == parameters.MedicineId);

            if (parameters.MinQuantity.HasValue)
                query = query.Where(r => r.Quantity >= parameters.MinQuantity);

            if (parameters.MaxQuantity.HasValue)
                query = query.Where(r => r.Quantity <= parameters.MaxQuantity);

            if (!string.IsNullOrWhiteSpace(parameters.Justification))
                query = query.Where(r => r.Justification != null && r.Justification.Contains(parameters.Justification));

            query = parameters.SortBy?.ToLower() switch
            {
                "id" => parameters.IsDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
                "medicine" => parameters.IsDescending? query.OrderByDescending(r => r.Medicine.Name) : query.OrderBy(r => r.Medicine.Name),
                "requestedByUser" => parameters.IsDescending? query.OrderByDescending(r => r.RequestedByUser.FirstName + " " + r.RequestedByUser.LastName) : query.OrderBy(r => r.RequestedByUser.FirstName + " " + r.RequestedByUser.LastName),
                "requiredByDate" => parameters.IsDescending? query.OrderByDescending(r => r.RequiredByDate) : query.OrderBy(r => r.RequiredByDate),
                "status" => parameters.IsDescending ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status),
                _ => parameters.IsDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<MedicineRequest>> GetByMedicineIdAsync(int medicineId)
        {
            return await _context.MedicineRequests
                .Where(r => r.MedicineId == medicineId)
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Medicine)
                .ToListAsync();
        }


        public async Task<List<MedicineRequest>> GetRequestsRequestedByUserIdAsync(int userId)
        {
            return await _context.MedicineRequests
                .Where(r => r.RequestedByUserId == userId)
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Medicine)
                .ToListAsync();
        }
        public async Task<List<MedicineRequest>> GetRequestsApprovedByUserIdAsync(int userId)
        {
            return await _context.MedicineRequests
                .Where(r => r.ApprovedByUserId == userId)
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Medicine)
                .ToListAsync();
        }

    }
}