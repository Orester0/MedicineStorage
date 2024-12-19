using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Helpers.Mappers
{
    public class AutoMapperMedicineOperations : Profile
    {
        public AutoMapperMedicineOperations()
        {

            CreateMap<MedicineUsage, ReturnMedicineUsageDTO>();
            CreateMap<CreateMedicineUsageDTO, MedicineUsage>();

            CreateMap<MedicineRequest, ReturnMedicineRequestDTO>();
            CreateMap<CreateMedicineRequestDTO, MedicineRequest>();




        }
    }
}
