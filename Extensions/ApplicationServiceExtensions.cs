using MedicineStorage.Data;
using MedicineStorage.Data.Implementations;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.ApplicationServices.Implementations;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Services.BusinessServices.Implementations;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;

namespace MedicineStorage.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            services.AddControllers();
            


            // SWAGGER
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            // SQL SERVER 
            services.AddDbContext<AppDbContext>(
                    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));


            // REPOSITORIES
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IMedicineOperationsService, MedicineOperationsService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<ITenderService, TenderService>();
    


            services.AddScoped<IUnitOfWork, UnitOfWork>();


            // APPLICATION SERVICES
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddSignalR();


            // BUSINESS SERVICES
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<IMedicineRequestRepository, MedicineRequestRepository>();
            services.AddScoped<IMedicineUsageRepository, MedicineUsageRepository>();
            services.AddScoped<ITenderProposalRepository, TenderProposalRepository>();
            services.AddScoped<ITenderRepository, TenderRepository>();
            services.AddScoped<ITenderProposalItemRepository, TenderProposalItemRepository>();
            services.AddScoped<ITenderItemRepository, TenderItemRepository>();
            services.AddScoped<IMedicineSupplyRepository, MedicineSupplyRepository>();



            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
       

    }
}
