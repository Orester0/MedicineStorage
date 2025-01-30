using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using MedicineStorage.Models.MedicineModels;
using AutoMapper;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class AuditService(IUnitOfWork _unitOfWork, IUserService _userService, IMapper _mapper) : IAuditService
    {

        public async Task<ServiceResult<PagedList<ReturnAuditDTO>>> GetAllAuditsAsync(AuditParams auditParams)
        {
            var result = new ServiceResult<PagedList<ReturnAuditDTO>>();
            var (audits, totalCount) = await _unitOfWork.AuditRepository.GetAllAuditsAsync(auditParams);
            result.Data = new PagedList<ReturnAuditDTO>(_mapper.Map<List<ReturnAuditDTO>>(audits), totalCount, auditParams.PageNumber, auditParams.PageSize);
            return result;
        }


        public async Task<ServiceResult<ReturnAuditDTO>> GetAuditByIdAsync(int auditId)
        {
            var result = new ServiceResult<ReturnAuditDTO>();
            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {

                throw new KeyNotFoundException($"Audit with ID {auditId} not found.");

            }

            result.Data = _mapper.Map<ReturnAuditDTO>(audit);


            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnAuditDTO>>> GetAuditsPlannedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnAuditDTO>>();


            var audits = await _unitOfWork.AuditRepository.GetAuditsByPlannedUserIdAsync(userId);
            if (audits == null)
            {

                throw new KeyNotFoundException($"Audits planned by user not found.");

            }

            result.Data = _mapper.Map<List<ReturnAuditDTO>>(audits);


            return result;
        }


        public async Task<ServiceResult<IEnumerable<ReturnAuditDTO>>> GetAuditsExecutedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnAuditDTO>>();

            var audits = await _unitOfWork.AuditRepository.GetAuditsByExecutedUserIdAsync(userId);
            if (audits == null)
            {
                throw new KeyNotFoundException($"Audits executed by user not found.");
            }

            result.Data = _mapper.Map<List<ReturnAuditDTO>>(audits);



            return result;
        }


        public async Task<ServiceResult<Audit>> CreateAuditAsync(int userId, CreateAuditDTO request)
        {
            var result = new ServiceResult<Audit>();

            var userRolesResult = await _userService.GetUserRolesAsync(userId);


            //var medicinesNeedingSpecialAudit = await CheckMedicinesRequireSpecialAuditAsync(request.MedicineIds, userRolesResult.Data ?? new List<string>());
            //if (medicinesNeedingSpecialAudit.Any())
            //{
            //    result.Errors.Add("Some medicines require Admin for audits planning");
            //    return result;
            //}

            var audit = new Audit
            {
                PlannedByUserId = userId,
                PlannedDate = request.PlannedDate,
                Status = AuditStatus.Planned,
                Notes = request.Notes
            };

            await _unitOfWork.AuditRepository.CreateAuditAsync(audit);
            await _unitOfWork.CompleteAsync();

            var auditItems = new List<AuditItem>();
            foreach (var medicineId in request.MedicineIds)
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                if (medicine == null)
                {

                    throw new KeyNotFoundException($"Medicine with ID {medicineId} not found");
                }

                var auditItem = new AuditItem
                {
                    AuditId = audit.Id,
                    MedicineId = medicineId,
                    ExpectedQuantity = medicine.MinimumStock,
                    ActualQuantity = 0
                };
                auditItems.Add(auditItem);
            }

            foreach (var item in auditItems)
            {
                await _unitOfWork.AuditRepository.CreateAuditItemAsync(item);
            }

            await _unitOfWork.CompleteAsync();

            result.Data = audit;
            return result;


        }
        public async Task<ServiceResult<Audit>> StartAuditAsync(int userId, int auditId, AuditNotes request)
        {
            var result = new ServiceResult<Audit>();
            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {

                throw new KeyNotFoundException($"Audit with ID {auditId} not found");

            }

            var userRolesResult = await _userService.GetUserRolesAsync(userId);


            if (audit.Status != AuditStatus.Planned)
            {

                throw new BadHttpRequestException($"Audit can only be started from Planned status");
            }

            var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(auditId);
            var medicineIds = auditItems.Select(ai => ai.MedicineId).ToArray();

            //var medicinesNeedingSpecialAudit = await CheckMedicinesRequireSpecialAuditAsync(medicineIds, userRolesResult.Data ?? new List<string>());
            //if (medicinesNeedingSpecialAudit.Any())
            //{
            //    result.Errors.Add("Some medicines require Supreme Manager for audits execution");
            //    return result;
            //}

            audit.ExecutedByUserId = userId;
            audit.StartDate = DateTime.UtcNow;
            audit.Status = AuditStatus.InProgress;
            audit.Notes = request.Notes;

            _unitOfWork.AuditRepository.UpdateAudit(audit);
            await _unitOfWork.CompleteAsync();

            result.Data = audit;
            return result;

        }

        public async Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int userId, int auditId, UpdateAuditItemsRequest request)
        {
            var result = new ServiceResult<Audit>();

            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {
                throw new KeyNotFoundException($"Audit with ID {auditId} not found");
            }

            if (audit.Status != AuditStatus.InProgress)
            {
                throw new BadHttpRequestException($"Audit can only be updated from Started status");
            }

            var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(auditId);

            foreach (var auditItem in auditItems)
            {
                if (request.ActualQuantities.TryGetValue(auditItem.MedicineId, out decimal actualQuantity))
                {
                    auditItem.ActualQuantity = actualQuantity;
                    _unitOfWork.AuditRepository.UpdateAuditItem(auditItem);
                }
            }

            bool hasSignificantDiscrepancies = auditItems.Any(item =>
                Math.Abs(item.ExpectedQuantity - item.ActualQuantity) >
                item.ExpectedQuantity * 0.05m);                                  // MOVE TO CONFIG

            audit.Status = hasSignificantDiscrepancies
                ? AuditStatus.RequiresFollowUp
                : AuditStatus.Completed;

            if (audit.Status == AuditStatus.Completed)
            {
                audit.EndDate = DateTime.UtcNow;
            }

            audit.Notes = request.Notes;

            _unitOfWork.AuditRepository.UpdateAudit(audit);
            await _unitOfWork.CompleteAsync();

            result.Data = audit;
            return result;

        }

        private async Task<IEnumerable<int>> CheckMedicinesRequireSpecialAuditAsync(int[] medicineIds, List<string> userRoles)
        {
            var specialAuditMedicines = new List<int>();

            foreach (var medicineId in medicineIds)
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);

                if (medicine == null)
                    continue;

                if (medicine.RequiresStrictAudit && !userRoles.Contains("Admin"))
                {
                    specialAuditMedicines.Add(medicineId);
                }
            }

            return specialAuditMedicines;
        }

        public async Task<ServiceResult<Audit>> CloseAuditAsync(int userId, int auditId, AuditNotes request)
        {
            var result = new ServiceResult<Audit>();

            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {
                throw new KeyNotFoundException($"Audit with ID {auditId} not found");
            }

            if (audit.Status != AuditStatus.Completed && audit.Status != AuditStatus.RequiresFollowUp)
            {

                throw new BadHttpRequestException($"Audit can only be closed when completed or requiring follow-up");
            }

            audit.ExecutedByUserId = userId;
            audit.EndDate ??= DateTime.UtcNow;
            audit.Status = AuditStatus.Completed;

            audit.Notes = request.Notes;

            _unitOfWork.AuditRepository.UpdateAudit(audit);
            await _unitOfWork.CompleteAsync();

            result.Data = audit;
            return result;

        }


        public async Task<ServiceResult<Audit>> UpdateAuditAsync(Audit audit)
        {
            var result = new ServiceResult<Audit>();

            var existingAudit = await _unitOfWork.AuditRepository.GetByIdAsync(audit.Id);
            if (existingAudit == null)
            {
                throw new KeyNotFoundException($"Audit not found");
            }

            _unitOfWork.AuditRepository.UpdateAudit(audit);
            await _unitOfWork.CompleteAsync();
            result.Data = audit;


            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAuditAsync(int auditId)
        {
            var result = new ServiceResult<bool>();

            var existingAudit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (existingAudit == null)
            {
                throw new KeyNotFoundException($"Audit not found");
            }

            await _unitOfWork.AuditRepository.DeleteAuditAsync(auditId);
            await _unitOfWork.CompleteAsync();
            result.Data = true;

            return result;
        }
    }
}