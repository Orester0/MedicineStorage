using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<bool>> UpdateRolesAsync(int userId, List<string> roleNames);
        Task UploadPhotoAsync(IFormFile file, int userId);
        Task<ServiceResult<User>> GetUserByIdAsync(int id);
        Task<ServiceResult<User>> GetByUserNameAsync(string username);
        Task<ServiceResult<List<User>>> GetAllAsync();
        Task<ServiceResult<PagedList<ReturnUserPersonalDTO>>> GetPaginatedUsers(UserParams parameters);
        Task<ServiceResult<List<User>>> GetUsersByRoleAsync(string roleName);
        Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto);
        Task<ServiceResult<bool>> UpdateUserAsync(User user);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
        Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId);
        Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> VerifyPasswordAsync(User user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> UserExists(string login);
        Task<bool> EmailTaken(string email);
        Task<ServiceResult<User>> RegisterUser(UserRegistrationDTO registerDto);
    }
}
