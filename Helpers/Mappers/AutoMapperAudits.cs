using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Helpers.Mappers
{
    public class AutoMapperAudits : Profile
    {
        public AutoMapperAudits()
        {
            CreateMap<Audit, ReturnAuditDTO>()
                .ForMember(dest => dest.PlannedByUser, opt => opt.MapFrom(src => src.PlannedByUser))
                .ForMember(dest => dest.ExecutedByUser, opt => opt.MapFrom(src => src.ExecutedByUser))
                .ForMember(dest => dest.AuditItems, opt => opt.MapFrom(src => src.AuditItems));

            CreateMap<AuditItem, ReturnAuditItemDTO>()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.Medicine));

        }
    }
}
