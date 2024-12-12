using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace MedicineStorage.Controllers
{
    //[Authorize(Policy = "SupremeAdmin")]
    public class SupremeAdminController(
         IUserService _userService,
         IMedicineService _medicineService,
         ITokenService _tokenService, 
         IMapper _mapper,
         ILogger<SupremeAdminController> _logger) : BaseApiController
    {

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDTO>>(users);
            return Ok(userDtos);
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound("User not found");

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDTO registerDto)
        {
            _logger.LogInformation($"Incoming manual user create request: \n{registerDto.ToJson()}");
            var result = await _userService.CreateUserAsync(registerDto);

            if (!result.Success)
                return BadRequest(result.Errors);

            var userDto = _mapper.Map<UserDTO>(result.Data);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Data.Id }, userDto);
        }

        [HttpPut("users/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO updateDto)
        {
            _logger.LogInformation($"Incoming user update request: \n{updateDto.ToJson()}");
            
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound("User not found");

            _mapper.Map(updateDto, user);
            var result = await _userService.UpdateUserAsync(user);

            if (!result.Success)
                return BadRequest(result.Errors);

            return NoContent();
        }


        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.Success)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpPost("users/{id:int}/roles/{roleName}")]
        public async Task<IActionResult> AssignRole(int id, string roleName)
        {
            var result = await _userService.AssignRoleAsync(id, roleName);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpDelete("users/{id:int}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRole(int id, string roleName)
        {
            var result = await _userService.RemoveRoleAsync(id, roleName);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpGet("users/{id:int}/roles")]
        public async Task<IActionResult> GetUserRoles(int id)
        {
            var roles = await _userService.GetUserRolesAsync(id);
            return Ok(roles);
        }
    }
}
