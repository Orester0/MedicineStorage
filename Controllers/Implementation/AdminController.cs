using AutoMapper;
using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    [Authorize]
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

            return Ok(new { result.Data } );
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            var userDto = _mapper.Map<UserDTO>(result.Data);
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

            return Ok(new { result.Data } );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDTO registerDto)
        {

            var result = await _userService.CreateUserAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            var userDto = _mapper.Map<UserDTO>(result.Data);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Data!.Id }, userDto);
        }

        [HttpPut("users/{userId:int}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDTO updateDto)
        {

            var userResult = await _userService.GetUserByIdAsync(userId);
            if (!userResult.Success)
            {
                return BadRequest(new { userResult.Errors });
            }

            _mapper.Map(updateDto, userResult.Data);
            var result = await _userService.UpdateUserAsync(userResult.Data);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return NoContent();
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

        [HttpPost("users/{userId:int}/roles/{roleName}")]
        public async Task<IActionResult> AssignRole(int userId, string roleName)
        {
            var result = await _userService.AssignRoleAsync(userId, roleName);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok();
        }

        [HttpDelete("users/{userId:int}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRole(int userId, string roleName)
        {
            var result = await _userService.RemoveRoleAsync(userId, roleName);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok();
        }

    }
}
