﻿using AutoMapper;
using CloudinaryDotNet.Actions;
using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Services.BusinessServices.Implementations;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    public class AccountController(
        IUserService _userService,
        ITokenService _tokenService,
        IEmailService _emailService,
        IAuditService _auditService, 
        ITenderService _tenderService, 
        IMedicineRequestService _requestService,
        IMedicineUsageService _usageService,
        IMapper _mapper,
        ILogger<AccountController> _logger
        ) : BaseApiController
    {

        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> GetInformationAboutCurrentUser()
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _userService.GetPersonalUserByIdAsync(userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("audits/planned")]
        public async Task<IActionResult> GetAuditsPlannedByCurrentUser()
        {

            var userId = User.GetUserIdFromClaims();
            var result = await _auditService.GetAuditsPlannedByUserId(userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);

        }

        [Authorize]
        [HttpGet("audits/executed")]
        public async Task<IActionResult> GetAuditsExecutedByCurrentUser()
        {

            var userId = User.GetUserIdFromClaims();
            var result = await _auditService.GetAuditsExecutedByUserId(userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);

        }

        [Authorize]
        [HttpGet("requests/requested")]
        public async Task<IActionResult> GetRequestsRequestedByCurrentUser()
        {

            var userId = User.GetUserIdFromClaims();
            var result = await _requestService.GetRequestsRequestedByUserId(userId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);

        }


        [Authorize]
        [HttpGet("usages/created")]
        public async Task<IActionResult> GetUsagesByCurrentUser()
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _usageService.GetUsagesByUserIdAsync(userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("requests/approved")]
        public async Task<IActionResult> GetRequestsApprovedByCurrentUser()
        {

            var userId = User.GetUserIdFromClaims();
            var result = await _requestService.GetRequestsApprovedByUserId(userId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);

        }

        [Authorize]
        [HttpGet("tenders/awarded")]
        public async Task<IActionResult> GetTendersAwardedByCurrentUser()
        {

            var userId = User.GetUserIdFromClaims();
            var result = await _tenderService.GetTendersAwardedByUserId(userId);
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);

        }

        [Authorize]
        [HttpGet("proposals/created")]
        public async Task<IActionResult> GetProposalsCreatedByCurrentUser()
        {

            var userId = User.GetUserIdFromClaims();
            var result = await _tenderService.GetProposalsCreatedByUserId(userId);
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ReturnUserTokenDTO>> RefreshToken([FromBody] UserRefreshTokenDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }
            try
            {
                var tokens = await _tokenService.RefreshAccessToken(dto.RefreshToken);
                return Ok(tokens);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] UserRefreshTokenDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }
            await _tokenService.RevokeRefreshToken(dto.RefreshToken);
            return Ok();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadFileDTO file)
        {
            if (file.File == null || file.File.Length == 0)
            {
                return BadRequest("Couldnt reach file");
            }

            var userId = User.GetUserIdFromClaims();
            await _userService.UploadPhotoAsync(file.File, userId);

            return Ok();
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _userService.UpdateUserAsync(updateDto, userId);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok();
        }



        [HttpPost("register")]
        public async Task<ActionResult<ReturnUserLoginDTO>> Register([FromBody] UserRegistrationDTO request)
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

            var accessToken = await _tokenService.CreateAccessToken(result.Data);
            var refreshToken = await _tokenService.CreateRefreshToken(result.Data);

            var user = _mapper.Map<ReturnUserPersonalDTO>(result.Data);

            var returnUserLoginDTO = new ReturnUserLoginDTO
            {
                returnUserTokenDTO = new ReturnUserTokenDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                returnUserDTO = user
            };

            return Ok(returnUserLoginDTO);
        }


        [HttpPost("login")]
        public async Task<ActionResult<ReturnUserLoginDTO>> Login(UserLoginDTO request)
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

            if (!await _userService.VerifyPasswordAsync(userResult.Data, request.Password))
            {
                return Unauthorized(new { Errors = new[] { "Invalid username or password" } });
            }

            var accessToken = await _tokenService.CreateAccessToken(userResult.Data);
            var refreshToken = await _tokenService.CreateRefreshToken(userResult.Data);
            var userPersonal = _mapper.Map<ReturnUserPersonalDTO>(userResult.Data);

            var returnUserLoginDTO = new ReturnUserLoginDTO
            {
                returnUserTokenDTO = new ReturnUserTokenDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                returnUserDTO = userPersonal
            };

            return Ok(returnUserLoginDTO);
        }



        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok("Password changed successfully.");
        }



    }
}
