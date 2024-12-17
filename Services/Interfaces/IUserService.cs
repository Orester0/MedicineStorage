using MedicineStorage.DTOs;
using MedicineStorage.Models;

namespace MedicineStorage.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<User?>> GetByIdAsync(int id);
        Task<ServiceResult<User?>> GetByUserNameAsync(string username);
        Task<ServiceResult<List<User>>> GetAllAsync();
        Task<ServiceResult<List<User>>> GetUsersByRoleAsync(string roleName);
        Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto);
        Task<ServiceResult<bool>> UpdateUserAsync(User user);
        Task<ServiceResult<bool>> DeleteUserAsync(int id);
        Task<ServiceResult<bool>> AssignRoleAsync(int userId, string roleName);
        Task<ServiceResult<bool>> RemoveRoleAsync(int userId, string roleName);
        Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId);
        Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> VerifyPasswordAsync(User user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> UserExists(string login);
        Task<bool> EmailTaken(string email);
        Task<ServiceResult<User>> ValidateAndCreateUserAsync(UserRegistrationDTO registerDto);
    }
}
