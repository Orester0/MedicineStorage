using MedicineStorage.Data.Interfaces;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Data.Implementations
{
    public class UnitOfWork(
        AppDbContext _context,
        IAuditRepository _auditRepository,
        IMedicineRepository _medicineRepository,
        IMedicineRequestRepository _medicineRequestRepository,
        IMedicineUsageRepository _medicineUsageRepository,
        ITenderRepository _tenderRepository,
        ITenderItemRepository _tenderItemRepository,
        ITenderProposalRepository _tenderProposalRepository,
        ITenderProposalItemRepository _tenderProposalItemRepository,
        IMedicineSupplyRepository _medicineSupplyRepository
        ) : IUnitOfWork
    {
        public IAuditRepository AuditRepository => _auditRepository;

        public IMedicineRepository MedicineRepository => _medicineRepository;

        public IMedicineRequestRepository MedicineRequestRepository => _medicineRequestRepository;

        public IMedicineUsageRepository MedicineUsageRepository => _medicineUsageRepository;

        public ITenderRepository TenderRepository => _tenderRepository;

        public ITenderProposalRepository TenderProposalRepository => _tenderProposalRepository;



        public ITenderItemRepository TenderItemRepository => _tenderItemRepository;

        public ITenderProposalItemRepository TenderProposalItemRepository => _tenderProposalItemRepository;





        public IMedicineSupplyRepository MedicineSupplyRepository => _medicineSupplyRepository;






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
