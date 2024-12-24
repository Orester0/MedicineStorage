using MedicineStorage.Extensions;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace MedicineStorage.Services.ApplicationServices.SignalR
{
    public class UserHub(IUserService _userService, ILogger<UserHub> _logger) : Hub
    {
        private static ConcurrentDictionary<string, User> _onlineUsers = new ConcurrentDictionary<string, User>();
        public override async Task OnConnectedAsync()
        {
            try
            {
                int userId = Context.User.GetUserIdFromClaims();

                var usernameResult = await _userService.GetUserByIdAsync(userId);

                if (usernameResult.Success && usernameResult.Data != null)
                {
                    var user = usernameResult.Data;
                    _onlineUsers[Context.ConnectionId] = user;

                    _logger.LogInformation(user.ToString());

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
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
                return;
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

        public async Task SendOnlineUsersAsync()
        {
            await Clients.Caller.SendAsync("ReceiveOnlineUsers", _onlineUsers.Values);
        }
    }
}
