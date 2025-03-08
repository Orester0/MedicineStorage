using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class UserService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IBlobStorageService _blobStorageService,
        IConfiguration configuration) : IUserService
    {
        private readonly string DEFAULT_PHOTO_BLOB_NAME =
            configuration["AzureStorage:DefaultImagePath"]
            ?? throw new Exception("Default image path cannot be null.");

        private async Task<string?> GetBase64PhotoAsync(string? blobName)
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

        public async Task<ServiceResult<User>> RegisterUser(UserRegistrationDTO registerDto)
        {
            var result = new ServiceResult<User>();

            if (await UserExists(registerDto.UserName))
            {
                result.Errors.Add($"Username '{registerDto.UserName}' is taken");
            }

            if (await EmailTaken(registerDto.Email))
            {
                result.Errors.Add($"Email '{registerDto.Email}' is taken");
            }

            foreach (var role in registerDto.Roles)
            {
                if (role is "Manager" or "Admin")
                {
                    result.Errors.Add($"Cannot register as '{role}'");
                }

                if (!await RoleExistsAsync(role))
                {
                    result.Errors.Add($"Role '{role}' does not exist");
                }
            }

            if (result.Errors.Any())
            {
                return result;
            }

            return await CreateUserAsync(registerDto);
        }

        public async Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto)
        {
            var result = new ServiceResult<User>();
            var user = _mapper.Map<User>(registerDto);

            user.PhotoBlobName = DEFAULT_PHOTO_BLOB_NAME;

            var (createResult, createdUser) = await _unitOfWork.UserRepository.CreateUserAsync(user, registerDto.Password);
            if (!createResult.Succeeded)
            {
                result.Errors.AddRange(createResult.Errors.Select(e => e.Description));
                return result;
            }

            var roleResult = await _unitOfWork.UserRepository.AddToRolesAsync(createdUser, registerDto.Roles);
            if (!roleResult.Succeeded)
            {
                result.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = createdUser;
            return result;
        }

        public async Task<ServiceResult<List<ReturnUserGeneralDTO>>> GetAllAsync()
        {
            var result = new ServiceResult<List<ReturnUserGeneralDTO>>();
            var users = await _unitOfWork.UserRepository.GetUsersWithRolesAsync();
            
            var dtos = new List<ReturnUserGeneralDTO>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<ReturnUserGeneralDTO>(user);
                dto.PhotoBase64 = await GetBase64PhotoAsync(user.PhotoBlobName);
                dtos.Add(dto);
            }
            
            result.Data = dtos;
            return result;
        }

        public async Task<ServiceResult<PagedList<ReturnUserPersonalDTO>>> GetPaginatedUsers(UserParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnUserPersonalDTO>>();
            var (users, totalCount) = await _unitOfWork.UserRepository.GetPaginatedUsersAsync(parameters);
            
            var dtos = new List<ReturnUserPersonalDTO>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<ReturnUserPersonalDTO>(user);
                dto.PhotoBase64 = await GetBase64PhotoAsync(user.PhotoBlobName);
                dtos.Add(dto);
            }
            
            result.Data = new PagedList<ReturnUserPersonalDTO>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
            return result;
        }

        public async Task UploadPhotoAsync(IFormFile file, int userId)
        {
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            // Delete old photo if exists and it's not the default photo
            if (!string.IsNullOrEmpty(user.PhotoBlobName) && user.PhotoBlobName != DEFAULT_PHOTO_BLOB_NAME)
            {
                await _blobStorageService.DeletePhotoAsync(user.PhotoBlobName);
            }

            byte[] compressedImage = await CompressImageAsync(file);
            user.PhotoBlobName = await _blobStorageService.UploadPhotoAsync(compressedImage, file.FileName);

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        private async Task<byte[]> CompressImageAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            using (var image = await Image.LoadAsync(stream))
            {
                image.Mutate(x => x.Resize(128, 128));

                using (var ms = new MemoryStream())
                {
                    await image.SaveAsync(ms, new JpegEncoder { Quality = 85 });
                    return ms.ToArray();
                }
            }
        }

        public async Task<ServiceResult<User>> GetByIdAsync(int id)
        {
            var result = new ServiceResult<User>();
            var user = await _unitOfWork.UserRepository.GetUserByIdWithRolesAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            result.Data = user;
            return result;
        }

        public async Task<ServiceResult<User>> GetByUserNameAsync(string username)
        {
            var result = new ServiceResult<User>();
            var user = await _unitOfWork.UserRepository.GetByUserNameWithRolesAsync(username);

            if (user == null)
            {
                result.Errors.Add("User not found.");
                return result;
            }

            result.Data = user;
            return result;
        }

        public async Task<ServiceResult<ReturnUserPersonalDTO>> GetPersonalUserByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnUserPersonalDTO>();
            var user = await _unitOfWork.UserRepository.GetUserByIdWithRolesAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var dto = _mapper.Map<ReturnUserPersonalDTO>(user);
            dto.PhotoBase64 = await GetBase64PhotoAsync(user.PhotoBlobName);

            result.Data = dto;
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(UserUpdateDTO updateDto, int userId)
        {
            var result = new ServiceResult<bool>();
            var existingUser = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());

            if (existingUser == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            _mapper.Map(updateDto, existingUser);
            var updateResult = await _unitOfWork.UserRepository.UpdateAsync(existingUser);

            if (!updateResult.Succeeded)
            {
                result.Errors.AddRange(updateResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            var result = new ServiceResult<bool>();
            var user = await _unitOfWork.UserRepository.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var deleteResult = await _unitOfWork.UserRepository.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                result.Errors.AddRange(deleteResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateRolesAsync(int userId, List<string> roleNames)
        {
            var result = new ServiceResult<bool>();
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var allRoles = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            var invalidRoles = roleNames.Except(allRoles).ToList();

            if (invalidRoles.Any())
            {
                result.Errors.Add($"Invalid roles: {string.Join(", ", invalidRoles)}");
                return result;
            }

            var currentRoles = await _unitOfWork.UserRepository.GetUserRolesAsync(user);
            var rolesToRemove = currentRoles.Except(roleNames).ToList();
            var rolesToAdd = roleNames.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _unitOfWork.UserRepository.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    result.Errors.AddRange(removeResult.Errors.Select(e => e.Description));
                    return result;
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _unitOfWork.UserRepository.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    result.Errors.AddRange(addResult.Errors.Select(e => e.Description));
                    return result;
                }
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var result = new ServiceResult<bool>();
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var passwordResult = await _unitOfWork.UserRepository.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!passwordResult.Succeeded)
            {
                result.Errors.AddRange(passwordResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByEmailWithRolesAsync(email);
        }

        public async Task<bool> VerifyPasswordAsync(User user, string password)
        {
            return await _unitOfWork.UserRepository.CheckPasswordAsync(user, password);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _unitOfWork.UserRepository.RoleExistsAsync(roleName);
        }

        public async Task<bool> UserExists(string login)
        {
            return await _unitOfWork.UserRepository.UserExistsAsync(login);
        }

        public async Task<bool> EmailTaken(string email)
        {
            return await _unitOfWork.UserRepository.EmailTakenAsync(email);
        }
    }
}

