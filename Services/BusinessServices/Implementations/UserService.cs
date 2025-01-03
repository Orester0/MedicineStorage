﻿using AutoMapper;
using MedicineStorage.Data;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class UserService(UserManager<User> _userManager, RoleManager<AppRole> _roleManager, AppDbContext _context, IMapper _mapper) : IUserService
    {
        public async Task<ServiceResult<User>> GetUserByIdAsync(int id)
        {
            var result = new ServiceResult<User>();
            var user = await _userManager.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
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
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());


            if (user == null)
            {
                result.Errors.Add("UserModels not found.");
            }

            result.Data = user;

            return result;
        }

        public async Task<ServiceResult<List<UserDTO>>> GetAllAsync()
        {
            var result = new ServiceResult<List<UserDTO>>();
            var users = await _userManager.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ToListAsync();

            _mapper.Map(users, result.Data);

            return result;
        }

        public async Task<ServiceResult<List<UserDTO>>> GetUsersByRoleAsync(string roleName)
        {
            var result = new ServiceResult<List<UserDTO>>();
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role {roleName} not found");
            }

            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
                .ToListAsync();

            _mapper.Map(users, result.Data);

            return result;
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
            catch (Exception)
            {
                await transaction.RollbackAsync();
                result.Errors.Add($"Couldnt create user");
                return result;
            }
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(User user)
        {
            var result = new ServiceResult<bool>();
            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());

            if (existingUser == null)
            {
                result.Errors.Add("UserModels not found");
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

                throw new KeyNotFoundException("User not found");
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
                throw new KeyNotFoundException("User not found");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new KeyNotFoundException("Role doesnt exists");
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
                throw new KeyNotFoundException("User not found");
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

        public async Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId)
        {
            var result = new ServiceResult<List<string>>();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            result.Data = roles.ToList();


            return result;
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var result = new ServiceResult<bool>();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
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

    }

}

