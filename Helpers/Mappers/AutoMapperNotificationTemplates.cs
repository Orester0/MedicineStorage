using MedicineStorage.DTOs;
using AutoMapper;
using MedicineStorage.Models.TemplateModels;

namespace MedicineStorage.Helpers.Mappers
{
    public class AutoMapperNotificationTemplates : Profile
    {
        public AutoMapperNotificationTemplates() 
        {
            CreateMap<AuditTemplate, AuditTemplateDTO>()
               .ForMember(dest => dest.CreateDTO, opt => opt.MapFrom(src => src.CreateDTO))
               .ReverseMap();

            CreateMap<TenderTemplate, TenderTemplateDTO>()
                .ForMember(dest => dest.CreateDTO, opt => opt.MapFrom(src => src.CreateDTO))
                .ReverseMap();

            CreateMap<MedicineRequestTemplate, MedicineRequestTemplateDTO>()
                .ForMember(dest => dest.CreateDTO, opt => opt.MapFrom(src => src.CreateDTO))
                .ReverseMap();
        }
    }
}
