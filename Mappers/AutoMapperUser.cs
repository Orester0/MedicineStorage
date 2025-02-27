using AutoMapper;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperUser : Profile
    {
        public AutoMapperUser()
        {
            CreateMap<UserRegistrationDTO, User>()
               .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<User, ReturnUserPersonalDTO>()
               .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
               .ForMember(dest => dest.PhotoBase64, opt => opt.MapFrom(src =>
                   src.Photo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(src.Photo)}" : null));


            CreateMap<User, ReturnUserGeneralDTO>()
               .ForMember(dest => dest.PhotoBase64, opt => opt.MapFrom(src =>
                   src.Photo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(src.Photo)}" : null));

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        }


    }


}
