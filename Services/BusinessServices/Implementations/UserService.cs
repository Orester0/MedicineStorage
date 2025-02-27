using AutoMapper;
using MedicineStorage.Data;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class UserService(
     UserManager<User> _userManager,
     RoleManager<AppRole> _roleManager,
     AppDbContext _context,
     IMapper _mapper,
     IConfiguration configuration) : IUserService
    {

        private readonly string defaultImagePath =
            configuration["ProfileSettings:DefaultImagePath"]
            ?? throw new Exception("Default image path cannot be null.");

        public async Task<ServiceResult<List<User>>> GetAllAsync()
        {
            var result = new ServiceResult<List<User>>();
            var users = await _userManager.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ToListAsync();

            result.Data = users;
            return result;
        }

        public async Task<ServiceResult<PagedList<ReturnUserPersonalDTO>>> GetPaginatedUsers(UserParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnUserPersonalDTO>>();

            var query = _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.FirstName))
                query = query.Where(u => u.FirstName.Contains(parameters.FirstName));

            if (!string.IsNullOrWhiteSpace(parameters.LastName))
                query = query.Where(u => u.LastName.Contains(parameters.LastName));

            if (!string.IsNullOrWhiteSpace(parameters.UserName))
                query = query.Where(u => u.UserName.Contains(parameters.UserName));

            if (!string.IsNullOrWhiteSpace(parameters.Email))
                query = query.Where(u => u.Email.Contains(parameters.Email));

            if (!string.IsNullOrWhiteSpace(parameters.Position))
                query = query.Where(u => u.Position == parameters.Position);

            if (!string.IsNullOrWhiteSpace(parameters.Company))
                query = query.Where(u => u.Company.Contains(parameters.Company));


            if (parameters.Roles?.Any() == true)
            {
                query = query.Where(u => u.UserRoles.Any(ur => parameters.Roles.Contains(ur.Role.Name)));
            }


            query = parameters.SortBy?.ToLower() switch
            {
                "id" => parameters.IsDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                "firstname" => parameters.IsDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => parameters.IsDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                "username" => parameters.IsDescending ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName),
                _ => query.OrderBy(u => u.Id) 
            };

            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<ReturnUserPersonalDTO>>(users);

            result.Data = new PagedList<ReturnUserPersonalDTO>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);

            return result;
        }




        public async Task UploadPhotoAsync(IFormFile file, int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            byte[] compressedImage = await CompressImageAsync(file);
            user.Photo = compressedImage;

            await _userManager.UpdateAsync(user);
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

        

        public async Task<ServiceResult<List<User>>> GetUsersByRoleAsync(string roleName)
        {
            var result = new ServiceResult<List<User>>();
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

            result.Data = users;
            return result;
        }

        public async Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto)
        {
            var result = new ServiceResult<User>();
            var user = _mapper.Map<User>(registerDto);


            if (File.Exists(defaultImagePath))
            {
                user.Photo = await File.ReadAllBytesAsync(defaultImagePath);
            }

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
                throw ex;
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
        public async Task<ServiceResult<bool>> UpdateRolesAsync(int userId, List<string> roleNames)
        {
            var result = new ServiceResult<bool>();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var invalidRoles = roleNames.Except(allRoles).ToList();

            if (invalidRoles.Any())
            {
                result.Errors.Add($"Invalid roles: {string.Join(", ", invalidRoles)}");
                return result;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Except(roleNames).ToList();
            var rolesToAdd = roleNames.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    result.Errors.AddRange(removeResult.Errors.Select(e => e.Description));
                    return result;
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    result.Errors.AddRange(addResult.Errors.Select(e => e.Description));
                    return result;
                }
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

