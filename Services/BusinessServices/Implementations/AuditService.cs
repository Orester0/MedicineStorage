using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class AuditService(IUnitOfWork _unitOfWork, IUserService _userService) : IAuditService
    {

        public async Task<ServiceResult<PagedList<Audit>>> GetAllAuditsAsync(AuditParams auditParams)
        {
            var result = new ServiceResult<PagedList<Audit>>();
            try
            {
                var (audits, totalCount) = await _unitOfWork.AuditRepository.GetAllAuditsAsync(auditParams);
                result.Data = new PagedList<Audit>(audits.ToList(), totalCount, auditParams.PageNumber, auditParams.PageSize);
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving audits");
            }
            return result;
        }


        public async Task<ServiceResult<Audit>> GetAuditByIdAsync(int auditId)
        {
            var result = new ServiceResult<Audit>();

            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(auditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {auditId} not found");
                }
                else
                {
                    result.Data = audit;
                }
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving audits");
            }

            return result;
        }

        public async Task<ServiceResult<IEnumerable<Audit>>> GetAuditsPlannedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<Audit>>();

            try
            {
                var audits = await _unitOfWork.AuditRepository.GetAuditsByPlannedUserIdAsync(userId);
                if (audits == null)
                {
                    result.Errors.Add($"Audits for planned user ID {userId} not found");
                }
                else
                {
                    result.Data = audits;
                }
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving audits");
            }

            return result;
        }


        public async Task<ServiceResult<IEnumerable<Audit>>> GetAuditsExecutedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<Audit>>();

            try
            {
                var audits = await _unitOfWork.AuditRepository.GetAuditsByExecutedUserIdAsync(userId);
                if (audits == null)
                {
                    result.Errors.Add($"Audits for executed user ID {userId} not found");
                }
                else
                {
                    result.Data = audits;
                }
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving audits");
            }

            return result;
        }


        public async Task<ServiceResult<Audit>> CreateAuditAsync(int userId, CreateAuditRequest request)
        {
            var result = new ServiceResult<Audit>();
            try
            {
                var userRolesResult = await _userService.GetUserRolesAsync(userId);
                if (!userRolesResult.Success)
                {
                    result.Errors.AddRange(userRolesResult.Errors);
                    return result;
                }

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
                await _unitOfWork.Complete();

                var auditItems = new List<AuditItem>();
                foreach (var medicineId in request.MedicineIds)
                {
                    var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                    if (medicine == null)
                    {
                        result.Errors.Add($"Medicine with ID {medicineId} not found");
                        continue;
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

                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error creating audits");
                return result;
            }
        }
        public async Task<ServiceResult<Audit>> StartAuditAsync(int userId, int auditId, AuditNotes request)
        {
            var result = new ServiceResult<Audit>();
            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(auditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {auditId} not found");
                    return result;
                }

                var userRolesResult = await _userService.GetUserRolesAsync(userId);
                if (!userRolesResult.Success)
                {
                    result.Errors.AddRange(userRolesResult.Errors);
                    return result;
                }

                if (audit.Status != AuditStatus.Planned)
                {
                    result.Errors.Add("Audit can only be started from Planned status");
                    return result;
                }

                var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(auditId);
                var medicineIds = auditItems.Select(ai => ai.MedicineId).ToArray();

                var medicinesNeedingSpecialAudit = await CheckMedicinesRequireSpecialAuditAsync(medicineIds, userRolesResult.Data ?? new List<string>());
                if (medicinesNeedingSpecialAudit.Any())
                {
                    result.Errors.Add("Some medicines require Supreme Manager for audits execution");
                    return result;
                }

                audit.ExecutedByUserId = userId;
                audit.StartDate = DateTime.UtcNow;
                audit.Status = AuditStatus.InProgress;
                audit.Notes = request.Notes;

                _unitOfWork.AuditRepository.UpdateAudit(audit);
                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error starting audit");
                return result;
            }
        }

        public async Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int userId, int auditId, UpdateAuditItemsRequest request)
        {
            var result = new ServiceResult<Audit>();
            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(auditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {auditId} not found");
                    return result;
                }

                if (audit.Status != AuditStatus.InProgress)
                {
                    result.Errors.Add("Audit items can only be updated when audits is in progress");
                    return result;
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
                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error updating audits items");
                return result;
            }
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
            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(auditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {auditId} not found");
                    return result;
                }

                if (audit.Status != AuditStatus.Completed && audit.Status != AuditStatus.RequiresFollowUp)
                {
                    result.Errors.Add("Audit can only be closed when completed or requiring follow-up");
                    return result;
                }

                audit.ExecutedByUserId = userId;
                audit.EndDate ??= DateTime.UtcNow;
                audit.Status = AuditStatus.Completed;

                audit.Notes = request.Notes;

                _unitOfWork.AuditRepository.UpdateAudit(audit);
                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error closing audit");
                return result;
            }
        }

        public async Task<ServiceResult<Audit>> CreateAuditAsync(Audit audit)
        {
            var result = new ServiceResult<Audit>();

            try
            {
                await _unitOfWork.AuditRepository.CreateAuditAsync(audit);
                await _unitOfWork.Complete();
                result.Data = audit;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error creating audit");
            }

            return result;
        }

        public async Task<ServiceResult<Audit>> UpdateAuditAsync(Audit audit)
        {
            var result = new ServiceResult<Audit>();

            try
            {
                var existingAudit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(audit.Id);
                if (existingAudit == null)
                {
                    result.Errors.Add($"Audit with ID {audit.Id} not found");
                    return result;
                }

                _unitOfWork.AuditRepository.UpdateAudit(audit);
                await _unitOfWork.Complete();
                result.Data = audit;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error updating audit");
            }

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAuditAsync(int auditId)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var existingAudit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(auditId);
                if (existingAudit == null)
                {
                    result.Errors.Add($"Audit with ID {auditId} not found");
                    result.Data = false;
                    return result;
                }

                await _unitOfWork.AuditRepository.DeleteAuditAsync(auditId);
                await _unitOfWork.Complete(); // Commit changes
                result.Data = true;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error deleting audit");
                result.Data = false;
            }

            return result;
        }
    }
}