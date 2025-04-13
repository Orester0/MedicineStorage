using MedicineStorage.Data;
using MedicineStorage.Data.Implementations;
using MedicineStorage.Data.Interfaces;

using MedicineStorage.Helpers;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Patterns;
using MedicineStorage.Services.ApplicationServices.Implementations;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Services.BusinessServices.Implementations;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                options.AddPolicy("AllowAzureClient", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            services.AddControllers();
            


            // SWAGGER
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            // SQL SERVER 
            services.AddDbContext<AppDbContext>(
                    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));



            // BUSINESS SERVICES
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IMedicineRequestService, MedicineRequestService>();
            services.AddScoped<IMedicineUsageService, MedicineUsageService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<ITenderService, TenderService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IMedicineSupplyService, MedicineSupplyService>();
            services.AddScoped<ITemplateCheckService, TemplateCheckService>();
            services.AddScoped<IDeadlineDateCheckService, DeadlineDateCheckService>();





            // APPLICATION SERVICES
            services.AddScoped<INotificationTextFactory, NotificationTextFactory>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSignalR();
            services.AddHostedService<TimeCheckerBackgroundService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();

            // REPOSITORIES
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<IMedicineRequestRepository, MedicineRequestRepository>();
            services.AddScoped<IMedicineUsageRepository, MedicineUsageRepository>();
            services.AddScoped<ITenderProposalRepository, TenderProposalRepository>();
            services.AddScoped<ITenderRepository, TenderRepository>();
            services.AddScoped<ITenderProposalItemRepository, TenderProposalItemRepository>();
            services.AddScoped<ITenderItemRepository, TenderItemRepository>();
            services.AddScoped<IMedicineSupplyRepository, MedicineSupplyRepository>();
            services.AddScoped<ITemplateRepository<MedicineRequestTemplate>, MedicineRequestTemplateRepository>();
            services.AddScoped<ITemplateRepository<AuditTemplate>, AuditTemplateRepository>();
            services.AddScoped<ITemplateRepository<TenderTemplate>, TenderTemplateRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
       

    }
}
