using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Data.Implementations
{
    public class UnitOfWork(
        AppDbContext _context,
        IAuditRepository _auditRepository,
        IMedicineRepository _medicineRepository,
        IMedicineRequestRepository _medicineRequestRepository,
        IMedicineUsageRepository _medicineUsageRepository,
        IUserService _userRepository) : IUnitOfWork
    {
        public IAuditRepository AuditRepository => _auditRepository;

        public IMedicineRepository MedicineRepository => _medicineRepository;

        public IMedicineRequestRepository MedicineRequestRepository => _medicineRequestRepository;

        public IMedicineUsageRepository MedicineUsageRepository => _medicineUsageRepository;

        public IUserService UserRepository => _userRepository;

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public Task Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
