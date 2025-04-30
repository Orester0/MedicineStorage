using Microsoft.Data.SqlClient;

namespace MedicineStorage.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IAuditRepository AuditRepository { get; }
        IMedicineRepository MedicineRepository { get; }
        IMedicineRequestRepository MedicineRequestRepository { get; }
        IMedicineUsageRepository MedicineUsageRepository { get; }
        ITenderRepository TenderRepository { get; }
        ITenderProposalRepository TenderProposalRepository { get; }
        IUserRepository UserRepository { get; }

        ITenderProposalItemRepository TenderProposalItemRepository { get; }
        ITenderItemRepository TenderItemRepository { get; }
        IMedicineSupplyRepository MedicineSupplyRepository { get; }
        INotificationRepository NotificationRepository { get; }

        Task<bool> CompleteAsync();
        bool HasChanges();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        Task ExecuteSqlRawAsync(string sql);

        Task ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters);

    }
}
