using AutoMapper;
using MedicineStorage.Controllers.Interface;

using MedicineStorage.Extensions;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    public class UsersController(IUserService _userService, IMapper _mapper) : BaseApiController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            var users = _mapper.Map<List<ReturnUserGeneralDTO>>(result.Data);
            return Ok(users);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
