using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MedicineStorage.Services.SignalR
{
    public class UserHub(IUserService _userService) : Hub
    {
        private static ConcurrentDictionary<string, User> _onlineUsers = new ConcurrentDictionary<string, User>();
        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                var usernameResult = await _userService.GetByUserNameAsync(username);

                if (usernameResult.Success && usernameResult.Data != null)
                {
                    var user = usernameResult.Data;
                    _onlineUsers[Context.ConnectionId] = user;

                    await Clients.All.SendAsync("UserConnected", user.UserName);
                }
                else
                {
                    foreach (var error in usernameResult.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_onlineUsers.TryRemove(Context.ConnectionId, out var removedUser))
            {
                await Clients.All.SendAsync("UserDisconnected", removedUser.UserName);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public static ConcurrentDictionary<string, User> GetOnlineUsers()
        {
            return _onlineUsers;
        }
    }
}
