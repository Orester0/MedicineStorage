using MedicineStorage.Patterns;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface INotificationTextFactory
    {
        (string title, string message) GetNotificationText(NotificationType type, params object[] args);
    }
}
