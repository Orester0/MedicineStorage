using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using MedicineStorage.Models.MedicineModels;
using AutoMapper;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class AuditService(IUnitOfWork _unitOfWork, IMapper _mapper) : IAuditService
    {
        public async Task<ServiceResult<PagedList<ReturnAuditDTO>>> GetPaginatedAudits(AuditParams auditParams)
        {
            var result = new ServiceResult<PagedList<ReturnAuditDTO>>();
            var (audits, totalCount) = await _unitOfWork.AuditRepository.GetByParams(auditParams);
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

        // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // 

        public async Task<ServiceResult<ReturnAuditDTO>> CreateAuditAsync(int userId, CreateAuditDTO request)
        {
            var result = new ServiceResult<ReturnAuditDTO>();

            var audit = _mapper.Map<Audit>(request);

            audit.PlannedByUserId = userId;
            audit.Status = AuditStatus.Planned;
            audit.Notes = new List<AuditNote>();

            if (!string.IsNullOrEmpty(request.Notes))
            {
                audit.Notes.Add(new AuditNote
                {
                    Note = request.Notes,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.AuditRepository.AddAsync(audit);
            await _unitOfWork.CompleteAsync();

            var medicines = await _unitOfWork.MedicineRepository.GetByIdsAsync(request.MedicineIds);
            var medicinesDict = medicines.ToDictionary(m => m.Id);

            var auditItems = request.MedicineIds.Select(medicineId =>
            {
                if (!medicinesDict.TryGetValue(medicineId, out var medicine))
                {
                    throw new KeyNotFoundException($"Medicine with ID {medicineId} not found");
                }

                return new AuditItem
                {
                    AuditId = audit.Id,
                    MedicineId = medicineId,
                    ExpectedQuantity = medicine.MinimumStock,
                    ActualQuantity = 0
                };
            }).ToList();

            await _unitOfWork.AuditRepository.CreateAuditItemsAsync(auditItems);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnAuditDTO>(audit);
            return result;


        }

        public async Task<ServiceResult<Audit>> StartAuditAsync(int userId, int auditId, CreateAuditNoteDTO request)
        {
            var result = new ServiceResult<Audit>();
            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {
                throw new KeyNotFoundException($"Audit with ID {auditId} not found");
            }

            if (audit.Status != AuditStatus.Planned)
            {
                throw new BadHttpRequestException($"Audit can only be started from Planned status");
            }


            audit.StartDate = DateTime.UtcNow;
            audit.Status = AuditStatus.InProgress;
            if (!string.IsNullOrEmpty(request.Note))
            {
                audit.Notes.Add(new AuditNote
                {
                    AuditId = auditId,
                    Note = request.Note,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _unitOfWork.AuditRepository.Update(audit);
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
                throw new BadHttpRequestException($"Audit should be in progress.");
            }

            var auditItems = await _unitOfWork.AuditRepository.GetAuditItemsByAuditIdAsync(auditId);
            bool anyUpdated = false;

            foreach (var (medicineId, actualQuantity) in request.ActualQuantities)
            {
                var auditItem = auditItems.FirstOrDefault(ai => ai.MedicineId == medicineId);
                if (auditItem == null)
                {
                    throw new KeyNotFoundException($"AuditItem for Medicine ID {medicineId} not found in this audit");
                }

                if (auditItem.CheckedByUserId != null)
                {
                    throw new BadHttpRequestException($"Medicine with ID {medicineId} has already been checked.");
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                if (medicine == null)
                {
                    throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
                }

                medicine.Stock = actualQuantity;

                auditItem.ActualQuantity = actualQuantity;
                auditItem.CheckedByUserId = userId;
                _unitOfWork.AuditRepository.UpdateAuditItem(auditItem);
                anyUpdated = true;
            }

            if (!anyUpdated)
            {
                result.Data = audit;
                return result;
            }

            if (!string.IsNullOrEmpty(request.Notes))
            {
                audit.Notes.Add(new AuditNote
                {
                    AuditId = auditId,
                    Note = request.Notes,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _unitOfWork.AuditRepository.Update(audit);
            await _unitOfWork.CompleteAsync();

            result.Data = audit;
            return result;
        }

        public async Task<ServiceResult<Audit>> CloseAuditAsync(int userId, int auditId, CreateAuditNoteDTO request)
        {
            var result = new ServiceResult<Audit>();

            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {
                throw new KeyNotFoundException($"Audit with ID {auditId} not found");
            }

            if (audit.AuditItems.Any(item => item.CheckedByUserId == null))
            {
                throw new BadHttpRequestException("Audit can only be closed when all items are checked.");
            }

            audit.ClosedByUserId = userId;
            audit.EndDate ??= DateTime.UtcNow;

            bool hasSignificantDiscrepancies = audit.AuditItems.Any(item => item.ExpectedQuantity > item.ActualQuantity);
            audit.Status = hasSignificantDiscrepancies
                ? AuditStatus.CompletedWithProblems
                : AuditStatus.SuccesfullyCompleted;

            foreach (var auditItem in audit.AuditItems)
            {
                if (auditItem.MedicineId == null)
                {
                    continue;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(auditItem.MedicineId);
                if (medicine != null)
                {
                    medicine.Stock = auditItem.ActualQuantity;
                    medicine.LastAuditDate = DateTime.UtcNow;
                    _unitOfWork.MedicineRepository.Update(medicine);
                }
            }

            if (!string.IsNullOrEmpty(request.Note))
            {
                audit.Notes.Add(new AuditNote
                {
                    AuditId = auditId,
                    Note = request.Note,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _unitOfWork.AuditRepository.Update(audit);
            await _unitOfWork.CompleteAsync();

            result.Data = audit;
            return result;
        }


        public async Task<ServiceResult<bool>> DeleteAuditAsync(int auditId, int userId, List<string> userRoles)
        {
            var result = new ServiceResult<bool>();

            var audit = await _unitOfWork.AuditRepository.GetByIdAsync(auditId);
            if (audit == null)
            {
                throw new KeyNotFoundException($"Audit with ID {auditId} not found");
            }


            if (audit.PlannedByUserId != userId && !userRoles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }


            if (audit.Status != AuditStatus.Planned)
            {

                throw new BadHttpRequestException("Only planned audits can be deleted");
            }


            await _unitOfWork.AuditRepository.DeleteAsync(audit.Id);
            await _unitOfWork.CompleteAsync();
            result.Data = true;

            return result;
        }
    }
}