using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Helpers
{
    public class AutoMapperMedicines : Profile
    {
        public AutoMapperMedicines()
        {
            CreateMap<Medicine, MedicineDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.RequiresSpecialApproval, opt => opt.MapFrom(src => src.RequiresSpecialApproval))
                .ForMember(dest => dest.MinimumStock, opt => opt.MapFrom(src => src.MinimumStock))
                .ForMember(dest => dest.RequiresStrictAudit, opt => opt.MapFrom(src => src.RequiresStrictAudit))
                .ForMember(dest => dest.AuditFrequencyDays, opt => opt.MapFrom(src => src.AuditFrequencyDays));

            CreateMap<CreateMedicineDTO, Medicine>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.RequiresSpecialApproval, opt => opt.MapFrom(src => src.RequiresSpecialApproval))
                .ForMember(dest => dest.MinimumStock, opt => opt.MapFrom(src => src.MinimumStock))
                .ForMember(dest => dest.RequiresStrictAudit, opt => opt.MapFrom(src => src.RequiresStrictAudit))
                .ForMember(dest => dest.AuditFrequencyDays, opt => opt.MapFrom(src => src.AuditFrequencyDays));




            CreateMap<MedicineUsage, MedicineUsageDTO>()
                .ForMember(dest => dest.MedicineId, opt => opt.MapFrom(src => src.MedicineId))
                .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name))
                .ForMember(dest => dest.UsedByUserId, opt => opt.MapFrom(src => src.UsedByUserId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UsageDate, opt => opt.MapFrom(src => src.UsageDate))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            CreateMap<CreateMedicineUsageDTO, MedicineUsage>()
                .ForMember(dest => dest.MedicineId, opt => opt.MapFrom(src => src.MedicineId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UsedByUserId, opt => opt.MapFrom(src => src.RequestedByUserId))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        }
    }
}
