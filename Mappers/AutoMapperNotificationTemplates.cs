using MedicineStorage.DTOs;
using AutoMapper;
using MedicineStorage.Models.TemplateModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperNotificationTemplates : Profile
    {
        public AutoMapperNotificationTemplates()
        {
            CreateMap<AuditTemplate, AuditTemplateDTO>()
               .ForMember(a => a.CreateDTO, opt => opt.MapFrom(src => src.CreateDTO))
               .ReverseMap();

            CreateMap<TenderTemplate, TenderTemplateDTO>()
               .ForMember(a => a.CreateDTO, opt => opt.MapFrom(src => src.CreateDTO))
                .ReverseMap();

            CreateMap<MedicineRequestTemplate, MedicineRequestTemplateDTO>()
               .ForMember(a => a.CreateDTO, opt => opt.MapFrom(src => src.CreateDTO))
                .ReverseMap();

            CreateMap<CreateAuditTemplate, CreateAuditDTO>()
               .ReverseMap();

            CreateMap<CreateTenderTemplate, CreateTenderDTO>()
                .ReverseMap();

            CreateMap<CreateMedicineRequestTemplate, CreateMedicineRequestDTO>()
                .ReverseMap();
        }
    }
}
