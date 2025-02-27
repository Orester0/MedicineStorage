using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers;
using Microsoft.AspNetCore.Mvc;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    [Route("api/medicine-request")]
    public class MedicineRequestController(IMedicineRequestService _operationsService) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<PagedList<ReturnMedicineRequestDTO>>> GetRequests([FromQuery] MedicineRequestParams parameters)
        {
            var result = await _operationsService.GetPaginatedAudits(parameters);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("{requestId:int}")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            var result = await _operationsService.GetRequestByIdAsync(requestId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("requested-by/{userId:int}")]
        public async Task<IActionResult> GetRequestsRequestedByUser(int userId)
        {
            var result = await _operationsService.GetRequestsRequestedByUserId(userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        [HttpPost("create")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateMedicineRequestDTO createRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
                var userId = User.GetUserIdFromClaims();
                var result = await _operationsService.CreateRequestAsync(createRequestDto, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpPut("approve/{requestId:int}")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            
                var userId = User.GetUserIdFromClaims();
                var userRoles = User.GetUserRolesFromClaims();
                var result = await _operationsService.ApproveRequestAsync(requestId, userId, userRoles);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpPut("reject/{requestId:int}")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            
                var userId = User.GetUserIdFromClaims();
                var userRoles = User.GetUserRolesFromClaims();
                bool isAdmin = userRoles.Contains("Admin");
                var result = await _operationsService.RejectRequestAsync(requestId, userId, userRoles);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpDelete("{requestId:int}")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {

            var userId = User.GetUserIdFromClaims();
            var roles = User.GetUserRolesFromClaims();
            var result = await _operationsService.DeleteRequestAsync(requestId, userId, roles);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }
    }
}
