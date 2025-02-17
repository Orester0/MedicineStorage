using MedicineStorage.Services.ApplicationServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class TemplateExecutionBackgroundService : BackgroundService
    {


        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TemplateExecutionBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunTemplateExecutionCheckAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await RunTemplateExecutionCheckAsync();
                }
            }
        }

        private async Task RunTemplateExecutionCheckAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var templateExecutionService = scope.ServiceProvider.GetRequiredService<ITemplateExecutionService>();

                await templateExecutionService.CheckAndNotifyAsync();
            }
        }
    }
}
