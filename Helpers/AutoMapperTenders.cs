using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Helpers
{
    public class AutoMapperTenders : Profile
    {
        public AutoMapperTenders()
        {
            CreateMap<CreateTenderDTO, Tender>();
            CreateMap<Tender, ReturnTenderDTO>();
                
        }
    }
}
