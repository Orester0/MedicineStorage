

using MedicineStorage.Models.TemplateModels;

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


        ITenderProposalItemRepository TenderProposalItemRepository { get; }
        ITenderItemRepository TenderItemRepository { get; }


        IMedicineSupplyRepository MedicineSupplyRepository { get; }
        INotificationRepository NotificationRepository { get; }


        ITemplateRepository<MedicineRequestTemplate> MedicineRequestTemplateRepository { get; }
        ITemplateRepository<AuditTemplate> AuditTemplateRepository { get; }
        ITemplateRepository<TenderTemplate> TenderTemplateRepository { get; }


        Task<bool> CompleteAsync();

        bool HasChanges();
        void BeginTransaction();
        Task Rollback();

    }
}
