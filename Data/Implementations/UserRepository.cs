using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Models.Params;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserRepository(
            AppDbContext context,
            UserManager<User> userManager,
            RoleManager<AppRole> roleManager) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetUsersWithRolesAsync()
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdWithRolesAsync(int id)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUserNameWithRolesAsync(string username)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
        }

        public async Task<User?> GetUserByEmailWithRolesAsync(string email)
        {
            return await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<(List<User>, int)> GetPaginatedUsersAsync(UserParams parameters)
        {
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

            return (users, totalCount);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
        }

        public async Task<bool> EmailTakenAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper());
        }

        public async Task<(IdentityResult Result, User User)> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result, user);
        }

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
        
        public async Task<List<string>> GetAllRoleNamesAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<User?> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }
    }
}
