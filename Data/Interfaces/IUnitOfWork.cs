using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IAuditRepository AuditRepository { get; }
        IMedicineRepository MedicineRepository { get; }
        IMedicineRequestRepository MedicineRequestRepository { get; }
        IMedicineUsageRepository MedicineUsageRepository { get; }
        ITenderProposalRepository TenderProposalRepository { get; }
        ITenderRepository TenderRepository { get; }
        IUserRepository UserRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
        void BeginTransaction();
        Task Rollback();

    }
}
