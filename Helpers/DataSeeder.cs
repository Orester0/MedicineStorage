using MedicineStorage.Models.DTOs;
using MedicineStorage.Services.BusinessServices.Implementations;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Helpers
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userService = services.GetRequiredService<IUserService>();


                var adminUserInfo = configuration.GetSection("AdminUserInfo").Get<List<string>>();

            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't seed data", ex);
            }
        }
    }
}
