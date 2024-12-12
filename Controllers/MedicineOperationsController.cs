using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    //[Authorize(Policy = "Admin")]
    public class MedicineOperationsController(IMedicineOperationsService _operationsService, ILogger<MedicineOperationsController> _logger) : BaseApiController
    {

        [HttpPut("requests/special-approval/{id:int}")]
        public async Task<IActionResult> ProcessSpecialApprovalRequest(int id, [FromBody] SpecialApprovalDTO approvalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _operationsService.ProcessSpecialApprovalRequestAsync(
                id,
                approvalDto.IsApproved,
                approvalDto.ApprovedByUserId
            );

            if (!result.Success)
                return result.Errors.Contains("Not found")
                    ? NotFound(result.Errors)
                    : BadRequest(result.Errors);

            return Ok(result.Data);
        }

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

        [HttpGet("requests/user/{userId:int}")]
        public async Task<IActionResult> GetRequestsByUser(int userId)
        {
            var result = await _operationsService.GetRequestsByUserAsync(userId);
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
            var result = await _operationsService.CreateRequestAsync(createRequestDto);
            if (!result.Success)
                return BadRequest(result.Errors);

            return CreatedAtAction(
                nameof(GetRequestById),
                new { id = result.Data.Id },
                result.Data
            );
        }

        [HttpPut("requests/{id:int}/status")]
        public async Task<IActionResult> UpdateRequestStatus(int id, [FromBody] UpdateRequestStatusDTO statusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _operationsService.UpdateRequestStatusAsync(id, statusDto);
            if (!result.Success)
                return result.Errors.Contains("Not found")
                    ? NotFound(result.Errors)
                    : BadRequest(result.Errors);

            return NoContent();
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

        [HttpPost("usage")]
        public async Task<IActionResult> CreateUsage([FromBody] CreateMedicineUsageDTO createUsageDto)
        {
            var result = await _operationsService.CreateUsageAsync(createUsageDto);
            if (!result.Success)
                return BadRequest(result.Errors);

            return CreatedAtAction(
                nameof(GetUsageById),
                new { id = result.Data.Id },
                result.Data
            );
        }

    }
}
