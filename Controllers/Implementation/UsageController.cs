using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    public class UsageController(IMedicineUsageService _usageService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<ReturnMedicineUsageDTO>>> GetUsages([FromQuery] MedicineUsageParams parameters)
        {
            var result = await _usageService.GetAllUsagesAsync(parameters);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }


        [HttpGet("{usageId:int}")]
        public async Task<IActionResult> GetUsageById(int usageId)
        {
            var result = await _usageService.GetUsageByIdAsync(usageId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }

        [HttpGet("created-by/{userId:int}")]
        public async Task<IActionResult> GetUsagesByUserId(int userId)
        {
            var result = await _usageService.GetUsagesByUserIdAsync(userId);
            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        [HttpPost]
        public async Task<IActionResult> CreateUsage([FromBody] CreateMedicineUsageDTO createUsageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
                var userId = User.GetUserIdFromClaims();
                var result = await _usageService.CreateUsageAsync(createUsageDto, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }
    }
}
