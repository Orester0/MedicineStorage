using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRequestRepository(AppDbContext _context)
    : GenericRepository<MedicineRequest>(_context), IMedicineRequestRepository
    {
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