﻿using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.Implementations;

namespace MedicineStorage.Services.Interfaces
{
    public interface IAuditService
    {

        public Task<ServiceResult<IEnumerable<Audit>>> GetAuditsExecutedByUserId(int userId);
        public Task<ServiceResult<IEnumerable<Audit>>> GetAuditsPlannedByUserId(int userId);
        public Task<ServiceResult<PagedList<Audit>>> GetAllAuditsAsync(AuditParams auditParams);
        public Task<ServiceResult<Audit>> GetAuditByIdAsync(int auditId);




        public Task<ServiceResult<Audit>> CreateAuditAsync(int userId, CreateAuditRequest request);
        public Task<ServiceResult<Audit>> StartAuditAsync(int userId, int auditId, StartAuditRequest request);
        public Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int userId, int auditId, UpdateAuditItemsRequest request);
        public Task<ServiceResult<Audit>> CloseAuditAsync(int userId, int auditId, CloseAuditRequest request);
        public Task<ServiceResult<Audit>> CreateAuditAsync(Audit audit);
        public Task<ServiceResult<Audit>> UpdateAuditAsync(Audit audit);
        public Task<ServiceResult<bool>> DeleteAuditAsync(int auditId);
    }
}
