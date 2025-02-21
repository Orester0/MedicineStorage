using AutoMapper;
using MedicineStorage.Controllers.Interface;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{

    //[Authorize(Roles = "Admin")]
    public class AdminController(IUserService _userService, IMapper _mapper) : BaseApiController
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            var userDto = _mapper.Map<ReturnUserDTO>(result.Data);
            return Ok(userDto);
        }

        [HttpGet("users/{userId:int}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var result = await _userService.GetUserRolesAsync(userId);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            var userDto = _mapper.Map<ReturnUserDTO>(result.Data);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Data!.Id }, userDto);
        }


        [HttpDelete("users/{userId:int}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {

            var result = await _userService.DeleteUserAsync(userId);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return NoContent();
        }

        [HttpPut("users/{userId:int}/roles")]
        public async Task<IActionResult> UpdateRoles(int userId, [FromBody] List<string> roleNames)
        {
            var result = await _userService.UpdateRolesAsync(userId, roleNames);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok();
        }


    }
}
