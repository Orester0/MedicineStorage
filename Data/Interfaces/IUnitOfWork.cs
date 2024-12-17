﻿using MedicineStorage.Data.Implementations;
using MedicineStorage.Services.Interfaces;

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


        Task<bool> Complete();

        bool HasChanges();
        void BeginTransaction();
        Task Rollback();

    }
}
