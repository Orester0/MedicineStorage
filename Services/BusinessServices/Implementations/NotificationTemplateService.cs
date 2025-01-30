using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class NotificationTemplateService(IUnitOfWork _unitOfWork, IMapper _mapper) : INotificationTemplateService
    {
        public async Task<IEnumerable<NotificationTemplateBaseDTO>> GetAllTenderTemplatesByUserIdAsync(int userId)
        {
            var medicineRequestTemplates = await _unitOfWork.MedicineRequestTemplateRepository.GetByUserIdAsync(userId);
            var auditTemplates = await _unitOfWork.AuditTemplateRepository.GetByUserIdAsync(userId);
            var tenderTemplates = await _unitOfWork.TenderTemplateRepository.GetByUserIdAsync(userId);

            var allTemplates = new List<NotificationTemplateBaseDTO>();

            allTemplates.AddRange(_mapper.Map<IEnumerable<MedicineRequestTemplateDTO>>(medicineRequestTemplates));
            allTemplates.AddRange(_mapper.Map<IEnumerable<AuditTemplateDTO>>(auditTemplates));
            allTemplates.AddRange(_mapper.Map<IEnumerable<TenderTemplateDTO>>(tenderTemplates));

            return allTemplates;
        }

        public async Task<MedicineRequestTemplateDTO> GetMedicineRequestTemplateByIdAsync(int id)
        {
            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(id);
            return _mapper.Map<MedicineRequestTemplateDTO>(template);
        }

        public async Task<AuditTemplateDTO> GetAuditTemplateByIdAsync(int id)
        {
            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(id);
            return _mapper.Map<AuditTemplateDTO>(template);
        }

        public async Task<TenderTemplateDTO> GetTenderTemplateByIdAsync(int id)
        {
            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(id);
            return _mapper.Map<TenderTemplateDTO>(template);
        }

        public async Task<IEnumerable<MedicineRequestTemplateDTO>> GetMedicineRequestTemplatesByUserIdAsync(int userId)
        {
            var templates = await _unitOfWork.MedicineRequestTemplateRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<MedicineRequestTemplateDTO>>(templates);
        }

        public async Task<IEnumerable<AuditTemplateDTO>> GetAuditTemplatesByUserIdAsync(int userId)
        {

            var templates = await _unitOfWork.AuditTemplateRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<AuditTemplateDTO>>(templates);
        }

        public async Task<IEnumerable<TenderTemplateDTO>> GetTenderTemplatesByUserIdAsync(int userId)
        {

            var templates = await _unitOfWork.TenderTemplateRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<TenderTemplateDTO>>(templates);
        }


        public async Task<MedicineRequestTemplate> CreateMedicineRequestTemplateAsync(MedicineRequestTemplateDTO dto, int userId)
        {
            var template = new MedicineRequestTemplate
            {
                UserId = userId,
                Name = dto.Name,
                RecurrenceInterval = dto.RecurrenceInterval,
                IsActive = true,
                LastExecutedDate = null,
                CreateDTO = dto.CreateDTO
            };
            await _unitOfWork.MedicineRequestTemplateRepository.AddAsync(template);
            await _unitOfWork.CompleteAsync();
            return template;
        }

        public async Task<AuditTemplate> CreateAuditTemplateAsync(AuditTemplateDTO dto, int userId)
        {
            var template = new AuditTemplate
            {
                UserId = userId,
                Name = dto.Name,
                RecurrenceInterval = dto.RecurrenceInterval,
                IsActive = true,
                LastExecutedDate = null,
                CreateDTO = dto.CreateDTO
            };
            await _unitOfWork.AuditTemplateRepository.AddAsync(template);
            await _unitOfWork.CompleteAsync();
            return template;
        }

        public async Task<TenderTemplate> CreateTenderTemplateAsync(TenderTemplateDTO dto, int userId)
        {
            var template = new TenderTemplate
            {
                UserId = userId,
                Name = dto.Name,
                RecurrenceInterval = dto.RecurrenceInterval,
                IsActive = true,
                LastExecutedDate = null,
                CreateDTO = dto.CreateDTO
            };
            await _unitOfWork.TenderTemplateRepository.AddAsync(template);
            await _unitOfWork.CompleteAsync();
            return template;
        }



        public async Task ExecuteMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.LastExecutedDate = DateTime.UtcNow;
            await _unitOfWork.MedicineRequestTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ExecuteAuditTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }
            template.LastExecutedDate = DateTime.UtcNow;
            await _unitOfWork.AuditTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ExecuteTenderTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }
            template.LastExecutedDate = DateTime.UtcNow;
            await _unitOfWork.TenderTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        // REVIEW

        public async Task<MedicineRequestTemplate> UpdateMedicineRequestTemplateAsync(int templateId, MedicineRequestTemplateDTO dto, int userId)
        {
            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.Name = dto.Name;
            template.RecurrenceInterval = dto.RecurrenceInterval;
            template.CreateDTO = dto.CreateDTO;

            await _unitOfWork.MedicineRequestTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
            return template;
        }

        public async Task<AuditTemplate> UpdateAuditTemplateAsync(int templateId, AuditTemplateDTO dto, int userId)
        {
            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.Name = dto.Name;
            template.RecurrenceInterval = dto.RecurrenceInterval;
            template.CreateDTO = dto.CreateDTO;

            await _unitOfWork.AuditTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
            return template;
        }

        public async Task<TenderTemplate> UpdateTenderTemplateAsync(int templateId, TenderTemplateDTO dto, int userId)
        {
            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.Name = dto.Name;
            template.RecurrenceInterval = dto.RecurrenceInterval;
            template.CreateDTO = dto.CreateDTO;

            await _unitOfWork.TenderTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
            return template;
        }

        public async Task DeactivateMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.IsActive = false;
            await _unitOfWork.MedicineRequestTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeactivateAuditTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.IsActive = false;
            await _unitOfWork.AuditTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeactivateTenderTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.IsActive = false;
            await _unitOfWork.TenderTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ActivateMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.IsActive = true;
            await _unitOfWork.MedicineRequestTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ActivateAuditTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.IsActive = true;
            await _unitOfWork.AuditTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ActivateTenderTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            template.IsActive = true;
            await _unitOfWork.TenderTemplateRepository.UpdateAsync(template);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteMedicineRequestTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.MedicineRequestTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            await _unitOfWork.MedicineRequestTemplateRepository.DeleteAsync(template.Id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAuditTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.AuditTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            await _unitOfWork.AuditTemplateRepository.DeleteAsync(template.Id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTenderTemplateAsync(int templateId, int userId)
        {
            var template = await _unitOfWork.TenderTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new KeyNotFoundException($"Template with ID {templateId} not found.");
            }

            if (template.UserId != userId)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            await _unitOfWork.TenderTemplateRepository.DeleteAsync(template.Id);
            await _unitOfWork.CompleteAsync();
        }

    }
}
