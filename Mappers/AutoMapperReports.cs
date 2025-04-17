using AutoMapper;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperReports : Profile
    {
        public AutoMapperReports()
        {
            CreateMap<Medicine, ReturnMedicineReportDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Audit, ReturnAuditReportDTO>();

            CreateMap<MedicineRequest, ReturnMedicineRequestReportDTO>()
                .ForMember(dest => dest.RequestedByUser, opt => opt.MapFrom(src => src.RequestedByUser))
                .ForMember(dest => dest.ApprovedByUser, opt => opt.MapFrom(src => src.ApprovedByUser));

            CreateMap<Tender, ReturnTenderReportDTO>();
        }
    }
}
