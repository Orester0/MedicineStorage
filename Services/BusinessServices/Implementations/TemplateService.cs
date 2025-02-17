using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class TemplateService(IUnitOfWork _unitOfWork, IMapper _mapper, 
        IMedicineRequestService _medicineOperationsService,
        ITenderService _tenderService,
        IAuditService _auditService) : ITemplateService
    {

        public async Task<ServiceResult<IEnumerable<MedicineRequestTemplateDTO>>> GetMedicineRequestTemplatesByUserIdAsync(int userId)
        {
            var result = new ServiceResult<IEnumerable<MedicineRequestTemplateDTO>>();

            var templates = await _unitOfWork.MedicineRequestTemplateRepository.GetByUserIdAsync(userId);
            foreach (var template in templates)
            {
                template.CreateDTO = JsonConvert.DeserializeObject<CreateMedicineRequestTemplate>(template.CreateDTOJson);
            }
            result.Data = _mapper.Map<List<MedicineRequestTemplateDTO>>(templates);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<AuditTemplateDTO>>> GetAuditTemplatesByUserIdAsync(int userId)
        {
            var result = new ServiceResult<IEnumerable<AuditTemplateDTO>>();

            var templates = await _unitOfWork.AuditTemplateRepository.GetByUserIdAsync(userId);
            foreach (var template in templates)
            {
                template.CreateDTO = JsonConvert.DeserializeObject<CreateAuditTemplate>(template.CreateDTOJson);
            }
            result.Data = _mapper.Map<List<AuditTemplateDTO>>(templates);

            return result;
        }

        public async Task<ServiceResult<IEnumerable<TenderTemplateDTO>>> GetTenderTemplatesByUserIdAsync(int userId)
        {
            var result = new ServiceResult<IEnumerable<TenderTemplateDTO>>();
            var templates = await _unitOfWork.TenderTemplateRepository.GetByUserIdAsync(userId);
            foreach (var template in templates)
            {
                template.CreateDTO = JsonConvert.DeserializeObject<CreateTenderTemplate>(template.CreateDTOJson);
            }
            result.Data = _mapper.Map<List<TenderTemplateDTO>>(templates);

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<ServiceResult<MedicineRequestTemplate>> CreateMedicineRequestTemplateAsync(MedicineRequestTemplateDTO dto, int userId)
        {
            var result = new ServiceResult<MedicineRequestTemplate>();

            var template = new MedicineRequestTemplate
            {
                UserId = userId,
                Name = dto.Name,
                RecurrenceInterval = dto.RecurrenceInterval,
                IsActive = true,
                LastExecutedDate = null,
                CreateDTOJson = JsonConvert.SerializeObject(dto.CreateDTO)
            };

            await _unitOfWork.MedicineRequestTemplateRepository.AddAsync(template);
            await _unitOfWork.CompleteAsync();

            result.Data = template;
            return result;
        }

        public async Task<ServiceResult<AuditTemplate>> CreateAuditTemplateAsync(AuditTemplateDTO dto, int userId)
        {
            var result = new ServiceResult<AuditTemplate>();

            var template = new AuditTemplate
            {
                UserId = userId,
                Name = dto.Name,
                RecurrenceInterval = dto.RecurrenceInterval,
                IsActive = true,
                LastExecutedDate = null,
                CreateDTOJson = JsonConvert.SerializeObject(dto.CreateDTO)
            };
            await _unitOfWork.AuditTemplateRepository.AddAsync(template);
            await _unitOfWork.CompleteAsync();

            result.Data = template;
            return result;
        }

        public async Task<ServiceResult<TenderTemplate>> CreateTenderTemplateAsync(TenderTemplateDTO dto, int userId)
        {
            var result = new ServiceResult<TenderTemplate>();

            var template = new TenderTemplate
            {
                UserId = userId,
                Name = dto.Name,
                RecurrenceInterval = dto.RecurrenceInterval,
                IsActive = true,
                LastExecutedDate = null,
                CreateDTOJson = JsonConvert.SerializeObject(dto.CreateDTO)
            };
            await _unitOfWork.TenderTemplateRepository.AddAsync(template);
            await _unitOfWork.CompleteAsync();

            result.Data = template;
            return result;
        }

        public async Task<ServiceResult<bool>> ExecuteMedicineRequestTemplateAsync(int templateId, int userId, DateTime requiredBy)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (template.IsActive == false)
            {
                throw new BadHttpRequestException("Couldnt execute deactivated template.");
            }

            var createDTO = JsonConvert.DeserializeObject<CreateMedicineRequestTemplate>(template.CreateDTOJson);

            var newTemplate = _mapper.Map<CreateMedicineRequestDTO>(createDTO);
            newTemplate.RequiredByDate = requiredBy;

            await _medicineOperationsService.CreateRequestAsync(newTemplate, userId);

            template.LastExecutedDate = DateTime.UtcNow;
            await _unitOfWork.MedicineRequestTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ExecuteAuditTemplateAsync(int templateId, int userId, DateTime plannedDate)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            
            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (template.IsActive == false)
            {
                throw new BadHttpRequestException("Couldnt execute deactivated template.");

            }

            var createDTO = JsonConvert.DeserializeObject<CreateAuditTemplate>(template.CreateDTOJson);

            var newTemplate = _mapper.Map<CreateAuditDTO>(createDTO);
            newTemplate.PlannedDate = plannedDate;

            await _auditService.CreateAuditAsync(userId, newTemplate);


            template.LastExecutedDate = DateTime.UtcNow;
            await _unitOfWork.AuditTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ExecuteTenderTemplateAsync(int templateId, int userId, DateTime deadlineDate)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }


            if (template.IsActive == false)
            {
                throw new BadHttpRequestException("Couldnt execute deactivated template.");
            }

            var createDTO = JsonConvert.DeserializeObject<CreateTenderTemplate>(template.CreateDTOJson);

            var newTemplate = _mapper.Map<CreateTenderDTO>(createDTO);
            newTemplate.DeadlineDate = deadlineDate;

            await _tenderService.CreateTenderAsync(newTemplate, userId);

            template.LastExecutedDate = DateTime.UtcNow;
            await _unitOfWork.TenderTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<MedicineRequestTemplate>> UpdateMedicineRequestTemplateAsync(int templateId, MedicineRequestTemplateDTO dto, int userId)
        {
            var result = new ServiceResult<MedicineRequestTemplate>();

            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.Name = dto.Name;
            template.RecurrenceInterval = dto.RecurrenceInterval;
            template.CreateDTOJson = JsonConvert.SerializeObject(dto.CreateDTO);

            await _unitOfWork.MedicineRequestTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = template;
            return result;
        }

        public async Task<ServiceResult<AuditTemplate>> UpdateAuditTemplateAsync(int templateId, AuditTemplateDTO dto, int userId)
        {
            var result = new ServiceResult<AuditTemplate>();

            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.Name = dto.Name;
            template.RecurrenceInterval = dto.RecurrenceInterval;
            template.CreateDTOJson = JsonConvert.SerializeObject(dto.CreateDTO);

            await _unitOfWork.AuditTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = template;
            return result;
        }

        public async Task<ServiceResult<TenderTemplate>> UpdateTenderTemplateAsync(int templateId, TenderTemplateDTO dto, int userId)
        {
            var result = new ServiceResult<TenderTemplate>();

            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.Name = dto.Name;
            template.RecurrenceInterval = dto.RecurrenceInterval;
            template.CreateDTOJson = JsonConvert.SerializeObject(dto.CreateDTO);

            await _unitOfWork.TenderTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = template;
            return result;
        }

        public async Task<ServiceResult<bool>> DeactivateMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.IsActive = false;
            await _unitOfWork.MedicineRequestTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeactivateAuditTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.IsActive = false;
            await _unitOfWork.AuditTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeactivateTenderTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
 
            }

            template.IsActive = false;
            await _unitOfWork.TenderTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ActivateMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.IsActive = true;
            await _unitOfWork.MedicineRequestTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ActivateAuditTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.IsActive = true;
            await _unitOfWork.AuditTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ActivateTenderTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            template.IsActive = true;
            await _unitOfWork.TenderTemplateRepository.Update(template);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            await _unitOfWork.MedicineRequestTemplateRepository.DeleteAsync(template.Id);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAuditTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            await _unitOfWork.AuditTemplateRepository.DeleteAsync(template.Id);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteTenderTemplateAsync(int templateId, int userId)
        {
            var result = new ServiceResult<bool>();

            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            await _unitOfWork.TenderTemplateRepository.DeleteAsync(template.Id);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }
    }
}