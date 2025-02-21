using MedicineStorage.Services.ApplicationServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class TimeCheckerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TimeCheckerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunAllChecksAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await RunAllChecksAsync();
                }
            }
        }

        private async Task RunAllChecksAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var templateExecutionService = scope.ServiceProvider.GetRequiredService<ITemplateCheckService>();

                await templateExecutionService.CheckAndNotifyAsync();
            }
        }
    }
}
