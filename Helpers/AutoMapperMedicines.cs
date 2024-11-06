using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Helpers
{
    public class AutoMapperMedicines : Profile
    {
        public AutoMapperMedicines()
        {
            CreateMap<Medicine, MedicineDTO>();

            CreateMap<MedicineRequest, MedicineRequestDTO>()
                .ForMember(dest => dest.RequestedByUserName,
                    opt => opt.MapFrom(src => src.RequestedByUser.UserName))
                .ForMember(dest => dest.ApprovedByUserName,
                    opt => opt.MapFrom(src => src.ApprovedByUser.UserName))
                .ForMember(dest => dest.MedicineName,
                    opt => opt.MapFrom(src => src.Medicine.Name));

            CreateMap<MedicineUsage, MedicineUsageDTO>()
                .ForMember(dest => dest.UsedByUserName,
                    opt => opt.MapFrom(src => src.UsedByUser.UserName))
                .ForMember(dest => dest.MedicineName,
                    opt => opt.MapFrom(src => src.Medicine.Name));
        }
    }
}
