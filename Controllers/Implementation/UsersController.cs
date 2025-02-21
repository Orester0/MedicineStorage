using MedicineStorage.Controllers.Interface;

using MedicineStorage.Extensions;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    public class UsersController(IUserService _userService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            } 
            return Ok(result.Data);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
