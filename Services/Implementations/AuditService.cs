using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class AuditService(IMedicineUsageRepository _usageRepository, IStockRepository _stockRepository) : IAuditService
    {
        

        public async void LogMedicineUsage(MedicineUsage usage)
        {
            await _usageRepository.AddAsync(usage);
        }

        public async void PerformAudit()
        {
            var medicines = await _stockRepository.GetAllAsync();
            foreach (var stock in medicines)
            {
                if (stock.Quantity <= stock.Quantity)
                {
                    // Notify admin for restock
                    Console.WriteLine($"Medicine {stock.MedicineId} is below minimum level");
                }
            }
        }

        public void GenerateAuditReminder()
        {
            // You could integrate this with a background job scheduler like Hangfire
            Console.WriteLine("Audit reminder generated.");
        }
    }
}
