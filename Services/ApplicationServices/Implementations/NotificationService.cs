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

            await _unitOfWork.NotificationRepository.AddNotificationAsync(notification); 
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", notification);

            await _unitOfWork.CompleteAsync();
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            await _unitOfWork.NotificationRepository.AddNotificationAsync(notification);
            await _hubContext.Clients.Group(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification);


            await _unitOfWork.CompleteAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _unitOfWork.NotificationRepository.MarkAsReadAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }

}
