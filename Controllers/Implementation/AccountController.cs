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
        public async Task<ActionResult<UserTokenReturnDTO>> Register([FromBody] UserRegistrationDTO request)
        {
            _logger.LogInformation($"Incoming registration request: \n{request.ToJson()}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUser(request);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(new UserTokenReturnDTO
            {
                Token = await _tokenService.CreateToken(result.Data)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenReturnDTO>> Login(UserLoginDTO request)
        {
            _logger.LogInformation($"Incoming login request: \n{request.ToJson()}");


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userResult = await _userService.GetByUserNameAsync(request.UserName);

            if (!userResult.Success || userResult.Data == null)
            {
                return Unauthorized(new { Errors = new[] { "Invalid username" } });
            }

            var verifyResult = await _userService.VerifyPasswordAsync(userResult.Data, request.Password);

            if (!verifyResult)
            {
                return Unauthorized(new { Errors = new[] { "Invalid username" } });
            }

            return Ok(new UserTokenReturnDTO
            {
                Token = await _tokenService.CreateToken(userResult.Data)
            });
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO request)
        {

            _logger.LogInformation($"Incoming change-password request: \n{request.ToJson()}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok("Password changed successfully.");
        }
    }
}
