using MedicineStorage.Data;
using MedicineStorage.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MedicineStorage.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = true;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<AppDbContext>();

            var tokenKey = config["TokenKey"] ?? throw new Exception("Token key not found");

            if (tokenKey.Length < 64)
            {
                throw new Exception("Token key needs to be at least 64 characters long for security");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("SupremeAdmin", policy => policy.RequireRole("SupremeAdmin"));
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                    options.AddPolicy("Member", policy => policy.RequireRole("Member"));
                });

            return services;
        }

    }
}
