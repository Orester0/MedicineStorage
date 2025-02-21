using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.NotificationModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task MarkAsReadAsync(int id);
    }
}
