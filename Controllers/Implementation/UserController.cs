using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{
    public class UserController(IUserService _userService, IAuditService _auditService, ITenderService _tenderService, IMedicineOperationsService _operationsService) : BaseApiController
    {

        [HttpGet("info")]
        public async Task<IActionResult> GetInformationAboutCurrentUser()
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

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

        [HttpGet("requests/requested")]
        public async Task<IActionResult> GetRequestsRequestedByCurrentUser()
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _operationsService.GetRequestsRequestedByUserId(userId);
                if (!result.Success)
                    return BadRequest(new { result.Errors });

                return Ok(result.Data);
            
        }

        [HttpGet("requests/approved")]
        public async Task<IActionResult> GetRequestsApprovedByCurrentUser()
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _operationsService.GetRequestsApprovedByUserId(userId);
                if (!result.Success)
                    return BadRequest(new { result.Errors });

                return Ok(result.Data);
            
        }

        [HttpGet("tenders/awarded")]
        public async Task<IActionResult> GetTendersAwardedByCurrentUser()
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.GetTendersAwardedByUserId(userId);
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(result.Data);
            
        }

        [HttpGet("proposals/created")]
        public async Task<IActionResult> GetProposalsCreatedByCurrentUser()
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.GetProposalsCreatedByUserId(userId); 
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(result.Data);
            
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
