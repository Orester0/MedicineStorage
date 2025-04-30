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
app.UseMiddleware<RoleBasedConnectionMiddleware>();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await RoleSeeder.SeedRoles(services, builder.Configuration);
    await ApplicationAdminUserGenerator.CreateUser(services, builder.Configuration);
    await DbUsersGenerator.CreateDatabaseUsers(services, builder.Configuration);

    var storedProceduresGenerator = services.GetRequiredService<IStoredProceduresGenerator>();
    await storedProceduresGenerator.CreateUpdateMinimumStockProcedureAsync();
    await storedProceduresGenerator.CreateCheckMedicineRequestApprovalTriggerAsync();
    await storedProceduresGenerator.CreateCleanupUnusedCategoryTriggerAsync();
    await storedProceduresGenerator.CreateGetOrInsertCategoryProcedureAsync();
    await storedProceduresGenerator.CreateDailyJobForExpiredUpdatesAsync();
    await storedProceduresGenerator.CreateUpdateExpiredMedicineRequestsProcedureAsync();
    await storedProceduresGenerator.CreateUpdateExpiredTendersProcedureAsync();
}


app.Run();

