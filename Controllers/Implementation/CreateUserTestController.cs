using MedicineStorage.Controllers.Interface;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    public class CreateUserTestController(
        IUserService _userService,
        ITokenService _tokenService,
        ILogger<AccountController> _logger
        ) : BaseApiController
    {
        [HttpPost("add-user")]
        public async Task<ActionResult<UserReturnDTO>> AddUser([FromBody] UserRegistrationDTO registerDto)
        {
            _logger.LogInformation($"Incoming create user request: \n{registerDto.ToJson()}");

            if (await _userService.UserExists(registerDto.UserName))
            {
                return BadRequest(new { Errors = new[] { $"Username '{registerDto.UserName}' is taken" } });
            }

            if (await _userService.EmailTaken(registerDto.Email))
            {
                return BadRequest(new { Errors = new[] { $"Email '{registerDto.Email}' is taken" } });
            }

            foreach (var role in registerDto.Roles)
            {
                if (!await _userService.RoleExistsAsync(role))
                {
                    return BadRequest(new { Errors = new[] { $"Role '{role}' does not exist" } });
                }
            }

            var result = await _userService.CreateUserAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(new UserReturnDTO
            {
                UserName = result.Data.UserName,
                Token = await _tokenService.CreateToken(result.Data)
            });
        }
    }
}
