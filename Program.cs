using MedicineStorage.Data;
using MedicineStorage.Extensions;
using MedicineStorage.Middleware;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.SignalR;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowLocalhost");

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
app.MapHub<UserHub>("/userHub");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        var roles = new List<AppRole>()
        {
            new() {Name = "Doctor"},
            new() {Name = "Manager"},
            new() {Name = "Admin"},
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



app.Run();

