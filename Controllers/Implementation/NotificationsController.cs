using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    public class NotificationsController(INotificationService _notificationsService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.GetUserIdFromClaims();
            var notifications = await _notificationsService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            await _notificationsService.SendNotificationAsync(notification.UserId, notification.Title ?? "Admin", notification.Message);
            return Ok();
        }

        [HttpPost("mark-as-read/{id:int}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationsService.MarkAsReadAsync(id);
            return NoContent();
        }
    }
}
