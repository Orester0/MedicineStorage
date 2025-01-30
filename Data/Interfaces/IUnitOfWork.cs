
using MedicineStorage.DTOs;
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


        INotificationTemplateRepository<MedicineRequestTemplate> MedicineRequestTemplateRepository { get; }
        INotificationTemplateRepository<AuditTemplate> AuditTemplateRepository { get; }
        INotificationTemplateRepository<TenderTemplate> TenderTemplateRepository { get; }


        Task<bool> CompleteAsync();

        bool HasChanges();
        void BeginTransaction();
        Task Rollback();

    }
}
