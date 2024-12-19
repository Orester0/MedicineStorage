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
    public class UsageController(IMedicineOperationsService _operationsService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<ReturnMedicineUsageDTO>>> GetUsages([FromQuery] MedicineUsageParams parameters)
        {
            var result = await _operationsService.GetAllUsagesAsync(parameters);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        [HttpGet("{usageId:int}")]
        public async Task<IActionResult> GetUsageById(int usageId)
        {
            var result = await _operationsService.GetUsageByIdAsync(usageId);
            if (!result.Success)
                return NotFound(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet("from-request/{requestId:int}")]
        public async Task<IActionResult> GetUsagesByRequestId(int requestId)
        {
            var result = await _operationsService.GetUsagesByRequestIdAsync(requestId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        [HttpPost]
        public async Task<IActionResult> CreateUsage([FromBody] CreateMedicineUsageDTO createUsageDto)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _operationsService.CreateUsageAsync(createUsageDto, userId);
                if (!result.Success)
                    return BadRequest(new { result.Errors });

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
