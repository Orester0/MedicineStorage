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
using System.Security.Cryptography;
using System.Text;

namespace MedicineStorage.Controllers
{
    public class AccountController(
        IUserService _userService,
        //UserManager<User> _userManager, 
        //RoleManager<AppRole> _roleManager, 
        ITokenService _tokenService, 
        IMapper _mapper,
        IEmailService _emailservice) : BaseApiController
    {
        [HttpPost("register")] 
        public async Task<ActionResult<UserReturnDTO>> Register([FromBody]UserRegistrationDTO registerDto)
        {
            if (await _userService.UserExists(registerDto.UserName))
            {
                return BadRequest($"/{registerDto.UserName}/ is taken");
            }

            if (await _userService.EmailTaken(registerDto.Email))
            {
                return BadRequest($"/{registerDto.Email}/ is taken");
            }

            foreach (var role in registerDto.Roles)
            {
                if(role == "Admin" || role == "SupremeAdmin")
                {
                    return BadRequest($"Cannot register as '{role}'.");
                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Role '{role}' does not exist.");
                }


            }

            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRolesAsync(user, registerDto.Roles);
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            //await _emailservice.SendEmailAsync(user.Email, "Registration", "Succesfull registration");

            return new UserReturnDTO
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserReturnDTO>> Login(UserLoginDTO loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

            if (user == null || user.UserName == null)
            {
                return Unauthorized("Invalid UserName");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) 
            {
                return Unauthorized();
            }

            return new UserReturnDTO
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
            };
        }

    }
}
