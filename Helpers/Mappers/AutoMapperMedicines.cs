using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Helpers.Mappers
{
    public class AutoMapperMedicines : Profile
    {
        public AutoMapperMedicines()
        {
            CreateMap<Medicine, ReturnMedicineDTO>();

            CreateMap<CreateMedicineDTO, Medicine>();
        }
    }
}
