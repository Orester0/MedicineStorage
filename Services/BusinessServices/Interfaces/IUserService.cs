using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<User>> GetByIdAsync(int id);
        Task<ServiceResult<User>> GetByUserNameAsync(string username);


        Task<ServiceResult<bool>> UpdateRolesAsync(int userId, List<string> roleNames);
        Task UploadPhotoAsync(IFormFile file, int userId);
        Task<ServiceResult<ReturnUserPersonalDTO>> GetPersonalUserByIdAsync(int id);
        Task<ServiceResult<List<ReturnUserGeneralDTO>>> GetAllAsync();
        Task<ServiceResult<PagedList<ReturnUserPersonalDTO>>> GetPaginatedUsers(UserParams parameters);
        Task<ServiceResult<bool>> UpdateUserAsync(UserUpdateDTO updateDto, int userId);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
        Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> VerifyPasswordAsync(User user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> UserExists(string login);
        Task<bool> EmailTaken(string email);
        Task<ServiceResult<User>> RegisterUser(UserRegistrationDTO registerDto);
        Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto);
    }
}
