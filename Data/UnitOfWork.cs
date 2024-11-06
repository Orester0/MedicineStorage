using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data
{
    public class UnitOfWork(
        AppDbContext _context, 
        IAuditRepository _auditRepository, 
        IMedicineRepository _medicineRepository,
        IMedicineRequestRepository _medicineRequestRepository,
        IMedicineUsageRepository _medicineUsageRepository,
        ITenderProposalRepository _tenderProposalRepository,
        ITenderRepository _tenderRepository,
        IUserRepository _userRepository) : IUnitOfWork
    {
        public IAuditRepository AuditRepository => _auditRepository;

        public IMedicineRepository MedicineRepository => _medicineRepository;

        public IMedicineRequestRepository MedicineRequestRepository => _medicineRequestRepository;

        public IMedicineUsageRepository MedicineUsageRepository => _medicineUsageRepository;


        public ITenderProposalRepository TenderProposalRepository => _tenderProposalRepository;

        public ITenderRepository TenderRepository => _tenderRepository;

        public IUserRepository UserRepository => _userRepository;

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
