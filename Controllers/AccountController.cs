using AutoMapper;
using MedicineStorage.Data;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Cryptography;
using System.Text;

namespace MedicineStorage.Controllers
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
                if (role is "Admin" or "SupremeAdmin")
                {
                    return BadRequest(new { Errors = new[] { $"Cannot register as '{role}'" } });
                }
                if (!await _userService.RoleExistsAsync(role))
                {
                    return BadRequest(new { Errors = new[] { $"Role '{role}' does not exist" } });
                }
            }

            var result = await _userService.CreateUserAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(new { Errors = result.Errors });
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

            var user = await _userService.GetByUserNameAsync(loginDto.UserName);
            if (user == null)
            {
                return Unauthorized(new { Errors = new[] { "Invalid username" } });
            }

            var result = await _userService.VerifyPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Unauthorized(new { Errors = new[] { "Invalid password" } });
            }

            return Ok(new UserReturnDTO
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user)
            });
        }
    }
}
