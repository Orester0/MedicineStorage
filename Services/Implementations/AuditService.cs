using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class AuditService(IUnitOfWork _unitOfWork, IUserService _userService) : IAuditService
    {
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

                var medicinesNeedingSpecialAudit = await CheckMedicinesRequireSpecialAuditAsync(request.MedicineIds, userRolesResult.Data ?? new List<string>());
                if (medicinesNeedingSpecialAudit.Any())
                {
                    result.Errors.Add("Some medicines require Supreme Admin for audit planning");
                    return result;
                }

                var audit = new Audit
                {
                    PlannedByUserId = userId,
                    PlannedDate = request.PlannedDate,
                    Status = AuditStatus.Planned,
                    Notes = request.Notes
                };

                await _unitOfWork.AuditRepository.AddAsync(audit);
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
                        ExpectedQuantity = medicine.Stock,
                        ActualQuantity = 0
                    };
                    auditItems.Add(auditItem);
                }

                foreach (var item in auditItems)
                {
                    await _unitOfWork.AuditRepository.AddAuditItemAsync(item);
                }

                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error creating audit: {ex.Message}");
                return result;
            }
        }
        public async Task<ServiceResult<Audit>> StartAuditAsync(int userId, StartAuditRequest request)
        {
            var result = new ServiceResult<Audit>();
            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(request.AuditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {request.AuditId} not found");
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

                var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(request.AuditId);
                var medicineIds = auditItems.Select(ai => ai.MedicineId).ToArray();

                var medicinesNeedingSpecialAudit = await CheckMedicinesRequireSpecialAuditAsync(medicineIds, userRolesResult.Data ?? new List<string>());
                if (medicinesNeedingSpecialAudit.Any())
                {
                    result.Errors.Add("Some medicines require Supreme Admin for audit execution");
                    return result;
                }

                audit.ExecutedByUserId = userId;
                audit.StartDate = DateTime.UtcNow;
                audit.Status = AuditStatus.InProgress;
                audit.Notes = request.Notes;

                await _unitOfWork.AuditRepository.UpdateAsync(audit);
                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error starting audit: {ex.Message}");
                return result;
            }
        }

        public async Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int userId, UpdateAuditItemsRequest request)
        {
            var result = new ServiceResult<Audit>();
            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(request.AuditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {request.AuditId} not found");
                    return result;
                }

                if (audit.Status != AuditStatus.InProgress)
                {
                    result.Errors.Add("Audit items can only be updated when audit is in progress");
                    return result;
                }

                var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(request.AuditId);

                foreach (var auditItem in auditItems)
                {
                    if (request.ActualQuantities.TryGetValue(auditItem.MedicineId, out decimal actualQuantity))
                    {
                        auditItem.ActualQuantity = actualQuantity;
                        await _unitOfWork.AuditRepository.UpdateAuditItemAsync(auditItem);
                    }
                }

                bool hasSignificantDiscrepancies = auditItems.Any(item =>
                    Math.Abs(item.ExpectedQuantity - item.ActualQuantity) >
                    (item.ExpectedQuantity * 0.05m));                                  // MOVE TO CONFIG

                audit.Status = hasSignificantDiscrepancies
                    ? AuditStatus.RequiresFollowUp
                    : AuditStatus.Completed;

                if (audit.Status == AuditStatus.Completed)
                {
                    audit.EndDate = DateTime.UtcNow;
                }

                audit.Notes = request.Notes;

                await _unitOfWork.AuditRepository.UpdateAsync(audit);
                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error updating audit items: {ex.Message}");
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

                if (medicine.RequiresStrictAudit && !userRoles.Contains("SupremeAdmin"))
                {
                    specialAuditMedicines.Add(medicineId);
                }
            }

            return specialAuditMedicines;
        }

        public async Task<ServiceResult<Audit>> CloseAuditAsync(int userId, CloseAuditRequest request)
        {
            var result = new ServiceResult<Audit>();
            try
            {
                var audit = await _unitOfWork.AuditRepository.GetAuditByIdAsync(request.AuditId);
                if (audit == null)
                {
                    result.Errors.Add($"Audit with ID {request.AuditId} not found");
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

                await _unitOfWork.AuditRepository.UpdateAsync(audit);
                await _unitOfWork.Complete();

                result.Data = audit;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error closing audit: {ex.Message}");
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<Audit>>> GetAllAuditsAsync()
        {
            var result = new ServiceResult<IEnumerable<Audit>>();

            try
            {
                var audits = await _unitOfWork.AuditRepository.GetAllAuditsAsync();
                result.Data = audits;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error retrieving audits: {ex.Message}");
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
            catch (Exception ex)
            {
                result.Errors.Add($"Error retrieving audit: {ex.Message}");
            }

            return result;
        }

        public async Task<ServiceResult<Audit>> CreateAuditAsync(Audit audit)
        {
            var result = new ServiceResult<Audit>();

            try
            {
                await _unitOfWork.AuditRepository.AddAsync(audit);
                await _unitOfWork.Complete();
                result.Data = audit;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error creating audit: {ex.Message}");
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

                await _unitOfWork.AuditRepository.UpdateAsync(audit);
                await _unitOfWork.Complete();
                result.Data = audit;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error updating audit: {ex.Message}");
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

                await _unitOfWork.AuditRepository.DeleteAsync(auditId);
                await _unitOfWork.Complete(); // Commit changes
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error deleting audit: {ex.Message}");
                result.Data = false;
            }

            return result;
        }
    }
}