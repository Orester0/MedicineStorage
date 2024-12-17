using AutoMapper;
using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    //[Authorize(Policy = "SupremeAdmin")]
    public class SupremeAdminController(
         IUserService _userService,
         IMapper _mapper,
         ILogger<SupremeAdminController> _logger) : BaseApiController
    {

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            if (!result.Success)
                return BadRequest(result.Errors);

            var userDtos = _mapper.Map<List<UserDTO>>(result.Data);
            return Ok(userDtos);
        }

        [HttpGet("users/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Errors);

            var userDto = _mapper.Map<UserDTO>(result.Data);
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
            return CreatedAtAction(nameof(GetUserById), new { id = result.Data!.Id }, userDto);
        }

        [HttpPut("users/{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO updateDto)
        {
            _logger.LogInformation($"Incoming user update request: \n{updateDto.ToJson()}");

            var userResult = await _userService.GetByIdAsync(id);
            if (!userResult.Success)
                return NotFound(userResult.Errors);

            _mapper.Map(updateDto, userResult.Data);
            var result = await _userService.UpdateUserAsync(userResult.Data);

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
            var result = await _userService.GetUserRolesAsync(id);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }
    }
}
