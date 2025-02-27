using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class NotificationService(IUnitOfWork _unitOfWork, IHubContext<NotificationHub> _hubContext) : INotificationService
    {

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _unitOfWork.NotificationRepository.GetUserNotificationsAsync(userId);
        }

        public async Task SendNotificationAsync(int userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            var addedNotification = await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            await _hubContext.Clients.Group(notification.UserId.ToString()).SendAsync("ReceiveNotification", addedNotification);
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            var addedNotification = await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            await _hubContext.Clients.Group(notification.UserId.ToString()).SendAsync("ReceiveNotification", addedNotification);
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _unitOfWork.NotificationRepository.MarkAsReadAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }

}
