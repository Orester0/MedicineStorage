using MedicineStorage.Models.NotificationModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<Notification?> GetNotificationByIdAsync(int id);
        Task AddNotificationAsync(Notification notification);
        Task MarkAsReadAsync(int id);
    }
}
