using MedicineStorage.Data;
using MedicineStorage.Extensions;
using MedicineStorage.Middleware;
using MedicineStorage.Models;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Implementations;
using MedicineStorage.Services.Interfaces;
using MedicineStorage.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");


using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        var roles = new List<AppRole>()
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "SupremeAdmin"},
            new() {Name = "Distributor"},
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


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Ensure the database is created
        //SeedTestData(context); // Call the seeding method here
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the database.");
    }
}


app.Run();







static void SeedTestData(AppDbContext context)
{

    var medicine1 = new Medicine { Name = "Paracetamol", Description = "Pain relief medication", Stock = 100, Category = "Antiemetics", RequiresSpecialApproval = false, MinimumStock = 50, RequiresStrictAudit = false, AuditFrequencyDays = 30 };
    var medicine2 = new Medicine { Name = "Ibuprofen", Description = "Pain relief medication", Stock = 150, Category = "Anesthetics", RequiresSpecialApproval = true, MinimumStock = 60, RequiresStrictAudit = true, AuditFrequencyDays = 30 };
    var medicine3 = new Medicine { Name = "Islamint", Description = "Pain relief medication", Stock = 160, Category = "Antiparasitics", RequiresSpecialApproval = false, MinimumStock = 70, RequiresStrictAudit = false, AuditFrequencyDays = 30 };
    context.Medicines.AddRange(medicine1, medicine2, medicine3);
    context.SaveChanges();
}
