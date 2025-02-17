using MedicineStorage.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }
        public async Task SendNotification(string userId, string message, string title)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", title, message);
        }
    }
}
