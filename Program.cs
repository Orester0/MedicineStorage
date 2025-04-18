using MedicineStorage.Extensions;
using MedicineStorage.Helpers;
using MedicineStorage.Middleware;
using MedicineStorage.Services.ApplicationServices.Implementations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAzureClient");

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



await RoleSeeder.SeedRoles(app.Services, builder.Configuration);
await AdminUserGenerator.CreateUser(app.Services, builder.Configuration);


app.Run();

