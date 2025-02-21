using MedicineStorage.Data.Interfaces;

using MedicineStorage.Models.TemplateModels;
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
        IMedicineSupplyRepository _medicineSupplyRepository,
        INotificationRepository _notificationRepository,
        ITemplateRepository<MedicineRequestTemplate> _medicineRequestTemplateRepository,
        ITemplateRepository<AuditTemplate> _auditTemplateRepository,
        ITemplateRepository<TenderTemplate> _tenderTemplateRepository
        ) : IUnitOfWork
    {
        public IAuditRepository AuditRepository => _auditRepository;
        public IMedicineRepository MedicineRepository => _medicineRepository;
        public IMedicineRequestRepository MedicineRequestRepository => _medicineRequestRepository;
        public IMedicineUsageRepository MedicineUsageRepository => _medicineUsageRepository;
        public ITenderRepository TenderRepository => _tenderRepository;
        public ITenderProposalRepository TenderProposalRepository => _tenderProposalRepository;
        public INotificationRepository NotificationRepository => _notificationRepository;
        public ITenderItemRepository TenderItemRepository => _tenderItemRepository;
        public ITenderProposalItemRepository TenderProposalItemRepository => _tenderProposalItemRepository;
        public IMedicineSupplyRepository MedicineSupplyRepository => _medicineSupplyRepository;
        public ITemplateRepository<MedicineRequestTemplate> MedicineRequestTemplateRepository => _medicineRequestTemplateRepository;
        public ITemplateRepository<AuditTemplate> AuditTemplateRepository => _auditTemplateRepository;
        public ITemplateRepository<TenderTemplate> TenderTemplateRepository => _tenderTemplateRepository;


        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CompleteAsync()
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
