using AutoMapper;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Mappers
{
    public class AutoMapperUser : Profile
    {
        public AutoMapperUser()
        {
            CreateMap<UserRegistrationDTO, User>()
               .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
               .ForMember(dest => dest.PhotoBlobName, opt => opt.Ignore());

            CreateMap<User, ReturnUserPersonalDTO>()
               .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
               .ForMember(dest => dest.PhotoBase64, opt => opt.Ignore());

            CreateMap<User, ReturnUserGeneralDTO>()
               .ForMember(dest => dest.PhotoBase64, opt => opt.Ignore());

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.PhotoBlobName, opt => opt.Ignore());
        }
    }

    public class PhotoBlobResolver : IValueResolver<User, ReturnUserPersonalDTO, string?>, IValueResolver<User, ReturnUserGeneralDTO, string?>
    {
        private readonly IBlobStorageService _blobStorageService;

        public PhotoBlobResolver(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public string? Resolve(User source, ReturnUserPersonalDTO destination, string? destMember, ResolutionContext context)
        {
            return ResolvePhoto(source.PhotoBlobName).Result;
        }

        public string? Resolve(User source, ReturnUserGeneralDTO destination, string? destMember, ResolutionContext context)
        {
            return ResolvePhoto(source.PhotoBlobName).Result;
        }

        private async Task<string?> ResolvePhoto(string? blobName)
        {
            if (string.IsNullOrEmpty(blobName))
                return null;

            try
            {
                var photoBytes = await _blobStorageService.DownloadPhotoAsync(blobName);
                return $"data:image/jpeg;base64,{Convert.ToBase64String(photoBytes)}";
            }
            catch
            {
                return null;
            }
        }
    }
}
