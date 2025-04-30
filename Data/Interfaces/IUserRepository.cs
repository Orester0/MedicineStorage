using MedicineStorage.Models.UserModels;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MedicineStorage.Data.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetUsersByRoleAsync(string rolename);
        Task<List<User>> GetUsersWithRolesAsync();
        Task<User?> GetUserByIdWithRolesAsync(int id);
        Task<User?> GetByUserNameWithRolesAsync(string username);
        Task<User?> GetUserByEmailWithRolesAsync(string email);
        Task<(List<User>, int)> GetPaginatedUsersAsync(UserParams parameters);
        Task<bool> UserExistsAsync(string username);
        Task<bool> EmailTakenAsync(string email);
        Task<(IdentityResult Result, User User)> CreateUserAsync(User user, string password);
        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<bool> RoleExistsAsync(string roleName);
        Task<List<string>> GetAllRoleNamesAsync();
        Task<User?> FindByIdAsync(string userId);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
    }
}
