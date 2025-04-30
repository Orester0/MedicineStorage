using MedicineStorage.Models.DTOs;
using MedicineStorage.Services.BusinessServices.Implementations;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Helpers
{
    public static class ApplicationAdminUserGenerator
    {
        public static async Task CreateUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userService = services.GetRequiredService<IUserService>();


                var adminUserInfo = configuration.GetSection("AdminUserInfo").Get<List<string>>();

                if (adminUserInfo == null || adminUserInfo.Count < 6)
                    throw new Exception("AdminUserInfo config section is incomplete");

                var userExists = await userService.UserExistsAsync(adminUserInfo[2]);

                if (userExists)
                    return;

                var registerDTO = new UserRegistrationDTO
                {
                    FirstName = adminUserInfo[0],
                    LastName = adminUserInfo[1],
                    UserName = adminUserInfo[2],
                    Email = adminUserInfo[3],
                    Password = adminUserInfo[4],
                    Position = adminUserInfo[5],
                    Company = adminUserInfo.Count > 6 ? adminUserInfo[6] : null,
                    Roles = adminUserInfo.Count > 7 ? adminUserInfo.GetRange(7, adminUserInfo.Count - 7) : new List<string>()
                };

                await userService.CreateUserAsync(registerDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't create admin user", ex);
            }
        }
    }
}
