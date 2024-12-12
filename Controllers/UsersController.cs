using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicineStorage.Data;
using MedicineStorage.Models;
using Microsoft.AspNetCore.Authorization;
using MedicineStorage.DTOs;
using AutoMapper;
using MedicineStorage.Data.Interfaces;
using NuGet.Protocol;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Controllers
{
    public class UsersController(IUnitOfWork _unitOfWork,
        IUserService _userService,
        ITokenService _tokenService,
        ILogger<AccountController> _logger
        ) : BaseApiController
    {
        [HttpPost("add-user")]
        public async Task<ActionResult<UserReturnDTO>> AddUser([FromBody] UserRegistrationDTO registerDto)
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
    }
}
