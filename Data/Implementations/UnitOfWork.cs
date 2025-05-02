using MedicineStorage.Data.Interfaces;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        IUserRepository _userRepository
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
        public IUserRepository UserRepository => _userRepository;
        public async Task<bool> CompleteAsync()
        {

            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task ExecuteSqlRawAsync(string sql)
        {
            await _context.Database.ExecuteSqlRawAsync(sql);
        }

        public async Task ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            var parameterList = string.Join(", ", parameters.Select(p =>
                p.ParameterName.StartsWith("@") ? p.ParameterName : "@" + p.ParameterName));

            var command = $"EXEC {procedureName} {parameterList}";
            await _context.Database.ExecuteSqlRawAsync(command, parameters);
        }
    }
}
