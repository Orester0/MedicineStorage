using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Implementations;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MedicineStorage.Controllers.Implementation
{
    public class RequestController(IMedicineOperationsService _operationsService) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<PagedList<ReturnMedicineRequestDTO>>> GetRequests([FromQuery] MedicineRequestParams parameters)
        {
            var result = await _operationsService.GetAllRequestsAsync(parameters);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        [HttpGet("{requestId:int}")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            var result = await _operationsService.GetRequestByIdAsync(requestId);
            if (!result.Success)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("requested-by/{userId:int}")]
        public async Task<IActionResult> GetRequestsRequestedByUser(int userId)
        {
            var result = await _operationsService.GetRequestsRequestedByUserId(userId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        [HttpGet("approved-by/{userId:int}")]
        public async Task<IActionResult> GetRequestsApprovedByUser(int userId)
        {
            var result = await _operationsService.GetRequestsApprovedByUserId(userId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        [HttpGet("requests-for/{medicineId:int}")]
        public async Task<IActionResult> GetRequestsForMedicine(int medicineId)
        {
            var result = await _operationsService.GetRequestsForMedicineId(medicineId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        [HttpGet("created-from/{usageId:int}")]
        public async Task<IActionResult> GetRequestByUsageId(int usageId)
        {
            var result = await _operationsService.GetRequestByUsageIdAsync(usageId);
            if (!result.Success)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        [HttpPost("create")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateMedicineRequestDTO createRequestDto)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _operationsService.CreateRequestAsync(createRequestDto, userId);
                if (!result.Success)
                    return BadRequest(new { result.Errors });

                return CreatedAtAction(
                    nameof(GetRequestById),
                    new { id = result.Data.Id },
                    result.Data
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("approve/{requestId:int}")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var userRoles = User.GetUserRolesFromClaims();
                bool isAdmin = userRoles.Contains("Admin");
                var result = await _operationsService.ApproveRequestAsync(requestId, userId, isAdmin);
                if (!result.Success)
                    return result.Errors.Contains("Not found")
                        ? NotFound(result.Errors)
                        : BadRequest(result.Errors);

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("reject/{requestId:int}")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var userRoles = User.GetUserRolesFromClaims();
                bool isAdmin = userRoles.Contains("Admin");
                var result = await _operationsService.RejectRequestAsync(requestId, userId, isAdmin);
                if (!result.Success)
                    return result.Errors.Contains("Not found")
                        ? NotFound(result.Errors)
                        : BadRequest(result.Errors);

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("{requestId:int}")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            var result = await _operationsService.DeleteRequestAsync(requestId);
            if (!result.Success)
                return result.Errors.Contains("Not found")
                    ? NotFound(result.Errors)
                    : BadRequest(result.Errors);

            return NoContent();
        }
    }
}
