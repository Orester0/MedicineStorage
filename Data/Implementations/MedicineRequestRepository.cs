using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineRequestRepository(AppDbContext _context) : IMedicineRequestRepository
    {
        public async Task<MedicineRequest?> GetByIdAsync(int id)
        {
            return await _context.MedicineRequests
                .Include(r => r.Medicine)
                .Include(r => r.RequestedByUser)
                .Include(r => r.ApprovedByUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<MedicineRequest>> GetAllAsync()
        {
            return await _context.MedicineRequests
                .Include(r => r.Medicine)
                .Include(r => r.RequestedByUser)
                .ToListAsync();
        }

        public async Task<List<MedicineRequest>> GetPendingRequestsAsync()
        {
            return await _context.MedicineRequests
                .Include(r => r.Medicine)
                .Include(r => r.RequestedByUser)
                .Where(r => r.Status == RequestStatus.Pending || r.Status == RequestStatus.PedingWithSpecial)
                .ToListAsync();
        }

        public async Task<List<MedicineRequest>> GetRequestsByUserAsync(int userId)
        {
            return await _context.MedicineRequests
                .Include(r => r.Medicine)
                .Where(r => r.RequestedByUserId == userId)
                .ToListAsync();
        }

        public async Task<List<MedicineRequest>> GetRequestsByStatusAsync(RequestStatus status)
        {
            return await _context.MedicineRequests
                .Include(r => r.Medicine)
                .Include(r => r.RequestedByUser)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<MedicineRequest> AddRequestAsync(MedicineRequest request)
        {
            request.RequestDate = DateTime.UtcNow;
            _context.MedicineRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<MedicineRequest> UpdateRequestAsync(MedicineRequest request)
        {
            _context.MedicineRequests.Update(request);
            await _context.SaveChangesAsync();

            // Reload the entity to get the updated version with includes
            return await GetByIdAsync(request.Id) ?? request;
        }

        public async Task<MedicineRequest> UpdateRequestStatusAsync(int requestId, RequestStatus status, int? approvedByUserId = null)
        {
            var request = await _context.MedicineRequests.FindAsync(requestId);
            if (request == null)
                throw new InvalidOperationException("Request not found.");

            request.Status = status;
            if (status == RequestStatus.Approved)
            {
                request.ApprovalDate = DateTime.UtcNow;
                request.ApprovedByUserId = approvedByUserId;
            }

            await _context.SaveChangesAsync();

            // Reload the entity to get the updated version with includes
            return await GetByIdAsync(requestId) ?? request;
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _context.MedicineRequests.FindAsync(id);
            if (request != null)
            {
                _context.MedicineRequests.Remove(request);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<MedicineRequest?> GetRequestByUsageIdAsync(int usageId)
        {
            var usage = await _context.MedicineUsages
                .Where(u => u.Id == usageId)
                .Include(u => u.MedicineRequest)
                .FirstOrDefaultAsync();

            return usage?.MedicineRequest;
        }
    }
}