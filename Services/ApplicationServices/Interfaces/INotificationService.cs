using MedicineStorage.Models.NotificationModels;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task SendNotificationAsync(int userId, string title, string message);
        Task SendNotificationAsync(Notification notification);
        Task MarkAsReadAsync(int id);
    }
}
