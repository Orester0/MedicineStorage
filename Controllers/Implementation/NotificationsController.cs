using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Services.ApplicationServices.Implementations;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Services.BusinessServices.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

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
