using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    public class AccountController(
        IUserService _userService,
        ITokenService _tokenService,
        IEmailService _emailService,
        ILogger<AccountController> _logger
        ) : BaseApiController
    {


        [HttpPost("register")]
        public async Task<ActionResult<UserReturnDTO>> Register([FromBody] UserRegistrationDTO registerDto)
        {
            _logger.LogInformation($"Incoming registration request: \n{registerDto.ToJson()}");

            var result = await _userService.ValidateAndCreateUserAsync(registerDto);

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

        [HttpPost("login")]
        public async Task<ActionResult<UserReturnDTO>> Login(UserLoginDTO loginDto)
        {
            _logger.LogInformation($"Incoming login request: \n{loginDto.ToJson()}");

            var userResult = await _userService.GetByUserNameAsync(loginDto.UserName);

            if (!userResult.Success || userResult.Data == null)
            {
                return Unauthorized(new { Errors = new[] { "Invalid username" } });
            }

            var verifyResult = await _userService.VerifyPasswordAsync(userResult.Data, loginDto.Password);

            if (!verifyResult)
            {
                return Unauthorized(new { Errors = new[] { "Invalid password" } });
            }

            return Ok(new UserReturnDTO
            {
                UserName = userResult.Data.UserName,
                Token = await _tokenService.CreateToken(userResult.Data)
            });
        }
    }
}
