using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Patterns;
using MedicineStorage.Services.ApplicationServices.Implementations;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface ITemplateExecutionService
    {
        public Task CheckAndNotifyAsync();

    }
}
