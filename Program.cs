using MedicineStorage.Data;
using MedicineStorage.Extensions;
using MedicineStorage.Middleware;
using MedicineStorage.Models;
using MedicineStorage.Services.Implementations;
using MedicineStorage.Services.Interfaces;
using MedicineStorage.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    await CreateRoles(services);
}
catch (Exception ex)
{
    Console.WriteLine($"Error creating roles: {ex.Message}");
}


app.Run();


async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

    var roles = new List<AppRole>()
    {
        new() {Name = "Member"},
        new() {Name = "Admin"},
        new() {Name = "SupremeAdmin"},
    };

    foreach(var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role.Name))
        {
            await roleManager.CreateAsync(role);
        }
    }

}