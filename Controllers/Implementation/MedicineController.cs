using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    public class MedicineController(IMedicineService _medicineService) : BaseApiController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMedicines()
        {
            var result = await _medicineService.GetAllMedicinesAsync();
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("stock-forecast")]
        public async Task<IActionResult> GetMedicineStockForecast([FromQuery] bool considerRequests = false,
           [FromQuery] bool considerTenders = false)
        {
            var result = await _medicineService.GetMedicineStockForecast(considerRequests, considerTenders);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicines([FromQuery] MedicineParams parameters)
        {
            var result = await _medicineService.GetPaginatedMedicines(parameters);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("{medicineId:int}")]
        public async Task<IActionResult> GetMedicineById(int medicineId)
        {
            var result = await _medicineService.GetMedicineByIdAsync(medicineId);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineDTO createMedicineDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _medicineService.CreateMedicineAsync(createMedicineDTO);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpPut("{medicineId:int}")]
        public async Task<IActionResult> UpdateMedicine(int medicineId, [FromBody] UpdateMedicineDTO medicineDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _medicineService.UpdateMedicineAsync(medicineId, medicineDTO);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpDelete("{medicineId:int}")]
        public async Task<IActionResult> DeleteMedicine(int medicineId)
        {
            var userRoles = User.GetUserRolesFromClaims();

            if (!userRoles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            var result = await _medicineService.DeleteMedicineAsync(medicineId, userRoles);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }
    }
}
