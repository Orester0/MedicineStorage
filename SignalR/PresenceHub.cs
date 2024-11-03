using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MedicineStorage.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            /*var username = Context.User?.GetUsername();
            if (!string.IsNullOrEmpty(username))
            {
                await Clients.Others.SendAsync("UserIsOnline", username);
            }*/
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            /*await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());

            await base.OnDisconnectedAsync(exception);*/
        }
    }
}
