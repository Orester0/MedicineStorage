﻿using MedicineStorage.Data;
using MedicineStorage.Data.Implementations;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Services.Implementations;
using MedicineStorage.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

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
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<AppDbContext>(
                    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            services.AddScoped<ITokenService, TokenService>();


            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IMedicineRepository, MedicineRepository>();
            services.AddScoped<IMedicineRequestRepository, MedicineRequestRepository>();
            services.AddScoped<IMedicineUsageRepository, MedicineUsageRepository>();
            services.AddScoped<ITenderProposalRepository, TenderProposalRepository>();
            services.AddScoped<ITenderRepository, TenderRepository>();
            services.AddScoped<ITenderProposalItemRepository, TenderProposalItemRepository>();
            services.AddScoped<ITenderItemRepository, TenderItemRepository>();
            services.AddScoped<IMedicineSupplyRepository, MedicineSupplyRepository>();



            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IMedicineOperationsService, MedicineOperationsService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<ITenderService, TenderService>();
            services.AddScoped<IEmailService, EmailService>();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();


            return services;
        }


    }
}
