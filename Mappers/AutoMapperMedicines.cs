using AutoMapper;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperMedicines : Profile
    {
        public AutoMapperMedicines()
        {
            CreateMap<Medicine, ReturnMedicineDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));


            CreateMap<Medicine, ReturnMedicineShortDTO>();

            CreateMap<CreateMedicineDTO, Medicine>()
                .ForMember(dest => dest.Category, opt => opt.Ignore()); 
            CreateMap<UpdateMedicineDTO, Medicine>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<MedicineSupply, ReturnMedicineSupplyDTO>();
            CreateMap<CreateMedicineSupplyDTO, MedicineSupply>();
        }
    }
}
