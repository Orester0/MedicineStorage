using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.TemplateModels;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface ITemplateService
    {

        public Task<ServiceResult<IEnumerable<MedicineRequestTemplateDTO>>> GetMedicineRequestTemplatesByUserIdAsync(int userId);

        public Task<ServiceResult<IEnumerable<AuditTemplateDTO>>> GetAuditTemplatesByUserIdAsync(int userId);

        public Task<ServiceResult<IEnumerable<TenderTemplateDTO>>> GetTenderTemplatesByUserIdAsync(int userId);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Task<ServiceResult<MedicineRequestTemplate>> CreateMedicineRequestTemplateAsync(MedicineRequestTemplateDTO dto, int userId);

        public Task<ServiceResult<AuditTemplate>> CreateAuditTemplateAsync(AuditTemplateDTO dto, int userId);

        public Task<ServiceResult<TenderTemplate>> CreateTenderTemplateAsync(TenderTemplateDTO dto, int userId);

        public Task<ServiceResult<bool>> ExecuteMedicineRequestTemplateAsync(int templateId, int userId, DateTime date);

        public Task<ServiceResult<bool>> ExecuteAuditTemplateAsync(int templateId, int userId, DateTime date);

        public Task<ServiceResult<bool>> ExecuteTenderTemplateAsync(int templateId, int userId, DateTime date);

        public Task<ServiceResult<MedicineRequestTemplate>> UpdateMedicineRequestTemplateAsync(int templateId, MedicineRequestTemplateDTO dto, int userId);

        public Task<ServiceResult<AuditTemplate>> UpdateAuditTemplateAsync(int templateId, AuditTemplateDTO dto, int userId);

        public Task<ServiceResult<TenderTemplate>> UpdateTenderTemplateAsync(int templateId, TenderTemplateDTO dto, int userId);

        public Task<ServiceResult<bool>> DeactivateMedicineRequestTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> DeactivateAuditTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> DeactivateTenderTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> ActivateMedicineRequestTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> ActivateAuditTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> ActivateTenderTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> DeleteMedicineRequestTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> DeleteAuditTemplateAsync(int templateId, int userId);

        public Task<ServiceResult<bool>> DeleteTenderTemplateAsync(int templateId, int userId);
    }
}