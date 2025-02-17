using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperUser : Profile
    {
        public AutoMapperUser()
        {
            CreateMap<UserRegistrationDTO, User>()
               .ForMember(dest => dest.UserRoles, opt => opt.Ignore());


            CreateMap<User, ReturnUserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));

            CreateMap<ReturnUserDTO, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        }


    }


}
