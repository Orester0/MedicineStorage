using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models;

namespace MedicineStorage.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserKnownDTO>();

            CreateMap<UserRegistrationDTO, User>();

        }
    }
}
