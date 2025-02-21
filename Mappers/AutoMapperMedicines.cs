using AutoMapper;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperMedicines : Profile
    {
        public AutoMapperMedicines()
        {
            CreateMap<Medicine, ReturnMedicineDTO>();
            CreateMap<CreateMedicineDTO, Medicine>();
            CreateMap<UpdateMedicineDTO, Medicine>();
            CreateMap<MedicineSupply, ReturnMedicineSupplyDTO>();
            CreateMap<CreateMedicineSupplyDTO, MedicineSupply>();
        }
    }
}
