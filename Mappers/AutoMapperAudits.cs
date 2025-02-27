using AutoMapper;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperAudits : Profile
    {
        public AutoMapperAudits()
        {
            CreateMap<Audit, ReturnAuditDTO>()
               .ForMember(dest => dest.PlannedByUser, opt => opt.MapFrom(src => src.PlannedByUser))
               .ForMember(dest => dest.ClosedByUser, opt => opt.MapFrom(src => src.ClosedByUser))
               .ForMember(dest => dest.AuditItems, opt => opt.MapFrom(src => src.AuditItems))
               .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
               .ReverseMap();

            CreateMap<AuditItem, ReturnAuditItemDTO>()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.Medicine))
                .ReverseMap();

            CreateMap<AuditNote, ReturnAuditNoteDTO>().ReverseMap();

            CreateMap<CreateAuditNoteDTO, AuditNote>();

            CreateMap<CreateAuditDTO, Audit>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(_ => new List<AuditNote>()));



            CreateMap<UpdateAuditItemsRequest, AuditItem>()
                .ForMember(dest => dest.ActualQuantity, opt => opt.MapFrom(src => src.ActualQuantities));


        }
    }
}
