using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    [Route("api/medicine-usage")]
    public class MedicineUsageController(IMedicineUsageService _usageService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<ReturnMedicineUsageDTO>>> GetUsages([FromQuery] MedicineUsageParams parameters)
        {
            var result = await _usageService.GetPaginatedUsages(parameters);
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
