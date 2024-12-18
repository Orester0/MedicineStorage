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

            CreateMap<UserRegistrationDTO, User>()
               .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<UserKnownDTO, User>();

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        }


    }


}
