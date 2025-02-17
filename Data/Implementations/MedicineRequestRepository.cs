using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRequestRepository(AppDbContext _context) : IMedicineRequestRepository
    {
        public async Task<MedicineRequest?> GetByIdAsync(int id)
        {
            return await _context.MedicineRequests
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .Include(r => r.Medicine)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<(IEnumerable<MedicineRequest>, int)> GetAllAsync(MedicineRequestParams parameters)
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

            if (parameters.Status.HasValue)
                query = query.Where(r => r.Status == parameters.Status);

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
                "requestdate" => parameters.IsDescending
                    ? query.OrderByDescending(r => r.RequestDate)
                    : query.OrderBy(r => r.RequestDate),
                "requireddate" => parameters.IsDescending
                    ? query.OrderByDescending(r => r.RequiredByDate)
                    : query.OrderBy(r => r.RequiredByDate),
                "quantity" => parameters.IsDescending
                    ? query.OrderByDescending(r => r.Quantity)
                    : query.OrderBy(r => r.Quantity),
                "status" => parameters.IsDescending
                    ? query.OrderByDescending(r => r.Status)
                    : query.OrderBy(r => r.Status),
                _ => parameters.IsDescending
                    ? query.OrderByDescending(r => r.RequestDate)
                    : query.OrderBy(r => r.RequestDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (items, totalCount);
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



        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<MedicineRequest> AddAsync(MedicineRequest request)
        {
            await _context.MedicineRequests.AddAsync(request);
            return request;
        }

        public void Update(MedicineRequest request)
        {
            _context.MedicineRequests.Update(request);
        }

        public async Task DeleteAsync(int requestId)
        {
            var medicineRequest = await _context.MedicineRequests.FindAsync(requestId);
            if (medicineRequest != null)
            {
                _context.MedicineRequests.Remove(medicineRequest);

            }
        }

    }
}