using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class AuditService(IUnitOfWork _unitOfWork, IUserService _userService) : IAuditService
    {
        public async Task<ServiceResult<Audit>> StartAuditAsync(int userId, int[] medicineIds)
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

                var userRoles = userRolesResult.Data ?? new List<string>();
                if (!userRoles.Any())
                {
                    result.Errors.Add("Unauthorized user");
                    return result;
                }

                var medicinesNeedingSpecialAudit = await CheckMedicinesRequireSpecialAuditAsync(medicineIds, userRoles);
                if (medicinesNeedingSpecialAudit.Any())
                {
                    result.Errors.Add("Some medicines require Supreme Admin for audit");
                    return result;
                }

                var audit = new Audit
                {
                    ConductedByUserId = userId,
                    AuditDate = DateTime.UtcNow,
                    Status = AuditStatus.InProgress,
                    Notes = $"Audit started for {medicineIds.Length} medicines"
                };

                var auditItems = new List<AuditItem>();
                foreach (var medicineId in medicineIds)
                {
                    var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                    if (medicine == null)
                    {
                        result.Errors.Add($"Medicine with ID {medicineId} not found");
                        continue;
                    }

                    var auditItem = new AuditItem
                    {
                        MedicineId = medicineId,
                        ExpectedQuantity = medicine.Stock,
                        ActualQuantity = 0
                    };
                    auditItems.Add(auditItem);
                }

                await _unitOfWork.AuditRepository.AddAsync(audit);

                foreach (var item in auditItems)
                {
                    item.AuditId = audit.Id;
                    await _unitOfWork.AuditRepository.AddAuditItemAsync(item);
                }

                await _unitOfWork.Complete(); // Commit changes

                result.Data = audit;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error starting audit: {ex.Message}");
                return result;
            }
        }

        public async Task<ServiceResult<Audit>> UpdateAuditItemsAsync(int auditId, Dictionary<int, decimal> actualQuantities, int userId)
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

                //var userRolesResult = await _userService.GetUserRolesAsync(userId);
                //if (!userRolesResult.Success)
                //{
                //    result.Errors.AddRange(userRolesResult.Errors);
                //    return result;
                //}

                var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(auditId);

                foreach (var auditItem in auditItems)
                {
                    if (actualQuantities.TryGetValue(auditItem.MedicineId, out decimal actualQuantity))
                    {
                        auditItem.ActualQuantity = actualQuantity;
                        await _unitOfWork.AuditRepository.UpdateAuditItemAsync(auditItem);
                    }
                }

                bool hasSignificantDiscrepancies = auditItems.Any(item =>
                    Math.Abs(item.ExpectedQuantity - item.ActualQuantity) >
                    (item.ExpectedQuantity * 0.05m)); // CONFIG TOLERANCE

                audit.Status = hasSignificantDiscrepancies
                    ? AuditStatus.RequiresFollowUp
                    : AuditStatus.Completed;
                audit.Notes += $" Audit updated by user {userId}";

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

        public async Task<ServiceResult<Audit>> CloseAuditAsync(int auditId, int userId)
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

                //var userRolesResult = await _userService.GetUserRolesAsync(userId);
                //if (!userRolesResult.Success)
                //{
                //    result.Errors.AddRange(userRolesResult.Errors);
                //    return result;
                //}

                if (audit.Status != AuditStatus.Completed && audit.Status != AuditStatus.RequiresFollowUp)
                {
                    result.Errors.Add("Audit cannot be closed in current status");
                    return result;
                }

                audit.Status = AuditStatus.Completed;
                audit.Notes += $" Audit closed by user {userId}";

                await _unitOfWork.AuditRepository.UpdateAsync(audit);
                await _unitOfWork.Complete(); // Commit changes

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
                await _unitOfWork.Complete(); // Commit changes
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
                await _unitOfWork.Complete(); // Commit changes
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