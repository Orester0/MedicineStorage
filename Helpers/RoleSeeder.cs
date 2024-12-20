using MedicineStorage.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace MedicineStorage.Helpers
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                var roles = new List<AppRole>
            {
                new() { Name = "Doctor" },
                new() { Name = "Manager" },
                new() { Name = "Admin" },
                new() { Name = "Distributor" }
            };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating roles: {ex.Message}");
            }
        }


    }
}
