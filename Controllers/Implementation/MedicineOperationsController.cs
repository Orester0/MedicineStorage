using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MedicineStorage.Controllers.Implementation
{
    //[Authorize(Policy = "Admin")]
    public class MedicineOperationsController(IMedicineOperationsService _operationsService) : BaseApiController
    {

        [HttpGet("requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _operationsService.GetAllRequestsAsync();
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("requests/{id:int}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var result = await _operationsService.GetRequestByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("requests/user")]
        public async Task<IActionResult> GetRequestsByCurrentUser()
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _operationsService.GetRequestsByUserAsync(userId);
                if (!result.Success)
                    return BadRequest(result.Errors);

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("requests/user/{id:int}")]
        public async Task<IActionResult> GetRequestsByUser(int id)
        {
            var result = await _operationsService.GetRequestsByUserAsync(id);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("requests/status/{status}")]
        public async Task<IActionResult> GetRequestsByStatus(RequestStatus status)
        {
            var result = await _operationsService.GetRequestsByStatusAsync(status);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("requests")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateMedicineRequestDTO createRequestDto)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _operationsService.CreateRequestAsync(createRequestDto, userId);
                if (!result.Success)
                    return BadRequest(result.Errors);

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

        [HttpPut("requests/{id:int}/approve")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var userRoles = GetUserRolesFromClaims();
                bool isSupremeAdmin = userRoles.Contains("SupremeAdmin");
                var result = await _operationsService.ApproveRequestAsync(id, userId, isSupremeAdmin);
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

        [HttpPut("requests/{id:int}/reject")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var userRoles = GetUserRolesFromClaims();
                bool isSupremeAdmin = userRoles.Contains("SupremeAdmin");
                var result = await _operationsService.RejectRequestAsync(id, userId, isSupremeAdmin);
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

        [HttpDelete("requests/{id:int}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var result = await _operationsService.DeleteRequestAsync(id);
            if (!result.Success)
                return result.Errors.Contains("Not found")
                    ? NotFound(result.Errors)
                    : BadRequest(result.Errors);

            return NoContent();
        }

        [HttpGet("usage")]
        public async Task<IActionResult> GetAllUsages()
        {
            var result = await _operationsService.GetAllUsagesAsync();
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("usage/{id:int}")]
        public async Task<IActionResult> GetUsageById(int id)
        {
            var result = await _operationsService.GetUsageByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("usage/request/{requestId:int}")]
        public async Task<IActionResult> GetUsagesByRequestId(int requestId)
        {
            var result = await _operationsService.GetUsagesByRequestIdAsync(requestId);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("usage/request/from-usage/{usageId:int}")]
        public async Task<IActionResult> GetRequestByUsageId(int usageId)
        {
            var result = await _operationsService.GetRequestByUsageIdAsync(usageId);
            if (!result.Success)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("usage")]
        public async Task<IActionResult> CreateUsage([FromBody] CreateMedicineUsageDTO createUsageDto)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _operationsService.CreateUsageAsync(createUsageDto, userId);
                if (!result.Success)
                    return BadRequest(result.Errors);

                return CreatedAtAction(
                    nameof(GetUsageById),
                    new { id = result.Data.Id },
                    result.Data
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

    }
}
