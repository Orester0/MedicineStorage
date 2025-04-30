using MedicineStorage.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace MedicineStorage.Helpers
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                var roles = configuration.GetSection("Roles").Get<List<string>>();

                foreach (var roleName in roles)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new AppRole { Name = roleName });
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("Couldnt seed roles");
            }
        }
    }
}
