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
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<(IEnumerable<MedicineRequest>, int)> GetAllAsync(MedicineRequestParams parameters)
        {
            var query = _context.MedicineRequests
                .Include(r => r.RequestedByUser)
                .AsQueryable();

            if (parameters.FromDate.HasValue)
                query = query.Where(r => r.RequestDate >= parameters.FromDate);

            if (parameters.ToDate.HasValue)
                query = query.Where(r => r.RequestDate <= parameters.ToDate);

            if (parameters.Status.HasValue)
                query = query.Where(r => r.Status == parameters.Status);

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "date" => parameters.IsDescending ?
                        query.OrderByDescending(r => r.RequestDate) :
                        query.OrderBy(r => r.RequestDate),
                    "status" => parameters.IsDescending ?
                        query.OrderByDescending(r => r.Status) :
                        query.OrderBy(r => r.Status),
                    _ => query.OrderByDescending(r => r.RequestDate)
                };
            }

            query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                         .Take(parameters.PageSize);

            return (await query.ToListAsync(), totalCount);
        }

        public async Task<List<MedicineRequest>> GetRequestsRequestedByUserIdAsync(int userId)
        {
            return await _context.MedicineRequests
                .Where(r => r.RequestedByUserId == userId)
                .ToListAsync();
        }

        public async Task<List<MedicineRequest>> GetRequestsApprovedByUserIdAsync(int userId)
        {
            return await _context.MedicineRequests
                .Where(r => r.ApprovedByUserId == userId)
                .ToListAsync();
        }

        public async Task<List<MedicineRequest>> GetRequestsForMedicineIdAsync(int medicineId)
        {
            return await _context.MedicineRequests
                .Where(r => r.MedicineId == medicineId)
                .ToListAsync();
        }

        public async Task<MedicineRequest?> GetRequestByUsageIdAsync(int usageId)
        {
            var usage = await _context.MedicineUsages
                .Where(u => u.Id == usageId)
                .Include(u => u.MedicineRequest)
                .FirstOrDefaultAsync();

            return usage?.MedicineRequest;
        }



        public async Task<MedicineRequest> CreateRequestAsync(MedicineRequest request)
        {
            await _context.MedicineRequests.AddAsync(request);
            return request;
        }

        public void UpdateRequest(MedicineRequest request)
        {
            _context.MedicineRequests.Update(request);
        }

        public async Task DeleteRequestAsync(int requestId)
        {
            var medicineRequest = await _context.MedicineRequests.FindAsync(requestId);
            if (medicineRequest != null)
            {
                _context.MedicineRequests.Remove(medicineRequest);

            }
        }

    }
}