using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperMedicineOperations : Profile
    {
        public AutoMapperMedicineOperations()
        {
            CreateMap<MedicineRequest, ReturnMedicineRequestDTO>()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.Medicine))
                .ForMember(dest => dest.RequestedByUser, opt => opt.MapFrom(src => src.RequestedByUser))
                .ForMember(dest => dest.ApprovedByUser, opt => opt.MapFrom(src => src.ApprovedByUser));
            CreateMap<CreateMedicineRequestDTO, MedicineRequest>();

            CreateMap<MedicineUsage, ReturnMedicineUsageDTO>()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.Medicine))
                .ForMember(dest => dest.UsedByUser, opt => opt.MapFrom(src => src.UsedByUser));
            CreateMap<CreateMedicineUsageDTO, MedicineUsage>();





        }
    }
}
