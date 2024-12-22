using MedicineStorage.Controllers.Interface;
using MedicineStorage.Services.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MedicineStorage.Controllers.Implementation
{
    public class ServiceController(IHubContext<UserHub> _hubContext) : BaseApiController
    {
        [HttpGet]
        public IActionResult GetOnlineUsers()
        {
            var users = UserHub.GetOnlineUsers().Values.ToList();
            return Ok(users);
        }
    }
}
