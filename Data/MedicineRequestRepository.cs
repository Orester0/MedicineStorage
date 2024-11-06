using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data
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
                .Where(r => r.Status == RequestStatus.Pending || r.Status == RequestStatus.ApprovalRequired)
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

        public async Task<bool> CreateRequestAsync(MedicineRequest request)
        {
            try
            {
                request.RequestDate = DateTime.UtcNow;
                request.Status = request.Medicine.RequiresSpecialApproval ?
                    RequestStatus.ApprovalRequired : RequestStatus.Pending;

                _context.MedicineRequests.Add(request);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRequestStatusAsync(int requestId, RequestStatus status, int? approvedByUserId = null)
        {
            var request = await _context.MedicineRequests.FindAsync(requestId);
            if (request == null) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                request.Status = status;
                if (status == RequestStatus.Approved)
                {
                    request.ApprovalDate = DateTime.UtcNow;
                    request.ApprovedByUserId = approvedByUserId;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _context.MedicineRequests.FindAsync(id);
            if (request == null) return false;

            try
            {
                _context.MedicineRequests.Remove(request);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
