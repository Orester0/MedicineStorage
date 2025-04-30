using MedicineStorage.Data;
using System.Security.Claims;

namespace MedicineStorage.Middleware
{
    public class RoleBasedConnectionMiddleware(RequestDelegate _next, IConfiguration _configuration)
    {

        public async Task InvokeAsync(HttpContext context, IDbConnectionStringProvider provider)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var roles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
                string connectionString = roles.Contains("Admin")
                    ? _configuration.GetConnectionString("AdminConnection")
                    : (roles.Contains("Doctor") || roles.Contains("Manager") || roles.Contains("Distributor"))
                        ? _configuration.GetConnectionString("StandardConnection")
                        : _configuration.GetConnectionString("GuestConnection");
                provider.SetConnectionString(connectionString);
            }
            else
            {
                provider.SetConnectionString(_configuration.GetConnectionString("GuestConnection"));
            }
            await _next(context);
        }
    }
}