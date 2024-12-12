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
            CreateMap<CreateMedicineDTO, Medicine>();
            CreateMap<MedicineDTO, Medicine>();

            CreateMap<MedicineUsage, MedicineUsageDTO>();
            CreateMap<CreateMedicineUsageDTO, MedicineUsage>();

            CreateMap<MedicineRequest, MedicineRequestDTO>();
            CreateMap<CreateMedicineRequestDTO, MedicineRequest>();
            CreateMap<MedicineRequestDTO, MedicineRequest>();



        }
    }
}
