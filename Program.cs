using MedicineStorage.Data;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers;
using MedicineStorage.Middleware;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.ApplicationServices.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

await RoleSeeder.SeedRoles(app.Services);

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
app.MapHub<NotificationHub>("/notificationHub");



app.Run();

