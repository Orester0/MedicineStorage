﻿
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



        Task<bool> Complete();

        bool HasChanges();
        void BeginTransaction();
        Task Rollback();

    }
}
