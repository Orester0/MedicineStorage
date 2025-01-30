using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Services.BusinessServices.Implementations;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface INotificationTemplateService
    {
        public Task<IEnumerable<NotificationTemplateBaseDTO>> GetAllTenderTemplatesByUserIdAsync(int userId);

        public Task<MedicineRequestTemplateDTO> GetMedicineRequestTemplateByIdAsync(int id);

        public Task<AuditTemplateDTO> GetAuditTemplateByIdAsync(int id);

        public Task<TenderTemplateDTO> GetTenderTemplateByIdAsync(int id);

        public Task<IEnumerable<MedicineRequestTemplateDTO>> GetMedicineRequestTemplatesByUserIdAsync(int userId);

        public Task<IEnumerable<AuditTemplateDTO>> GetAuditTemplatesByUserIdAsync(int userId);

        public Task<IEnumerable<TenderTemplateDTO>> GetTenderTemplatesByUserIdAsync(int userId);


        public Task<MedicineRequestTemplate> CreateMedicineRequestTemplateAsync(MedicineRequestTemplateDTO dto, int userId);

        public Task<AuditTemplate> CreateAuditTemplateAsync(AuditTemplateDTO dto, int userId);

        public Task<TenderTemplate> CreateTenderTemplateAsync(TenderTemplateDTO dto, int userId);



        public Task ExecuteMedicineRequestTemplateAsync(int templateId, int userId);

        public Task ExecuteAuditTemplateAsync(int templateId, int userId);

        public Task ExecuteTenderTemplateAsync(int templateId, int userId);
        public Task<MedicineRequestTemplate> UpdateMedicineRequestTemplateAsync(int templateId, MedicineRequestTemplateDTO dto, int userId);

        public Task<AuditTemplate> UpdateAuditTemplateAsync(int templateId, AuditTemplateDTO dto, int userId);

        public Task<TenderTemplate> UpdateTenderTemplateAsync(int templateId, TenderTemplateDTO dto, int userId);

        public Task DeactivateMedicineRequestTemplateAsync(int templateId, int userId);

        public Task DeactivateAuditTemplateAsync(int templateId, int userId);

        public Task DeactivateTenderTemplateAsync(int templateId, int userId);

        public Task ActivateMedicineRequestTemplateAsync(int templateId, int userId);

        public Task ActivateAuditTemplateAsync(int templateId, int userId);

        public Task ActivateTenderTemplateAsync(int templateId, int userId);

        public Task DeleteMedicineRequestTemplateAsync(int templateId, int userId);

        public Task DeleteAuditTemplateAsync(int templateId, int userId);

        public Task DeleteTenderTemplateAsync(int templateId, int userId);

    }
}


