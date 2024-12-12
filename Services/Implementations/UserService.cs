using AutoMapper;
using MedicineStorage.Data;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, RoleManager<AppRole> roleManager,
            AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return new List<User>();

            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
                .ToListAsync();
        }

        public async Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto)
        {
            var result = new ServiceResult<User>();
            var user = _mapper.Map<User>(registerDto);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var createResult = await _userManager.CreateAsync(user, registerDto.Password);
                if (!createResult.Succeeded)
                {
                    result.Errors.AddRange(createResult.Errors.Select(e => e.Description));
                    return result;
                }

                var roleResult = await _userManager.AddToRolesAsync(user, registerDto.Roles);
                if (!roleResult.Succeeded)
                {
                    result.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                    await transaction.RollbackAsync();
                    return result;
                }

                await transaction.CommitAsync();
                result.Data = user;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(User user)
        {
            var result = new ServiceResult<bool>();
            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());

            if (existingUser == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            _mapper.Map(user, existingUser);
            var updateResult = await _userManager.UpdateAsync(existingUser);

            if (!updateResult.Succeeded)
            {
                result.Errors.AddRange(updateResult.Errors.Select(e => e.Description));
                return result;
            }

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            var result = new ServiceResult<bool>();
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                result.Errors.AddRange(deleteResult.Errors.Select(e => e.Description));
                return result;
            }

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> AssignRoleAsync(int userId, string roleName)
        {
            var result = new ServiceResult<bool>();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                result.Errors.Add("Role does not exist");
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                result.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return result;
            }

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> RemoveRoleAsync(int userId, string roleName)
        {
            var result = new ServiceResult<bool>();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            var roleResult = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                result.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return result;
            }

            result.Data = true;
            return result;
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var result = new ServiceResult<bool>();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            var passwordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!passwordResult.Succeeded)
            {
                result.Errors.AddRange(passwordResult.Errors.Select(e => e.Description));
                return result;
            }

            result.Data = true;
            return result;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> VerifyPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> UserExists(string login)
        {
            return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == login.ToUpper());
        }

        public async Task<bool> EmailTaken(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper());
        }
    }
}

