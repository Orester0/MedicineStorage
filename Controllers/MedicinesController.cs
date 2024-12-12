using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    //[Authorize(Policy = "Admin")]
    public class MedicinesController(IMedicineService _medicineService, ILogger<MedicinesController> _logger) : BaseApiController
    {
        private IActionResult HandleServiceResult<T>(ServiceResult<T> result, Func<T, IActionResult> onSuccess)
        {
            if (result.Success)
                return onSuccess(result.Data!);

            var errorMessages = string.Join("; ", result.Errors);

            _logger.LogWarning("Request failed: {Errors}", errorMessages);

            return result.Errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase))
                ? NotFound(result.Errors)
                : BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedicines()
        {
            var result = await _medicineService.GetAllMedicinesAsync();
            return HandleServiceResult(result, data => Ok(data));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMedicine(int id)
        {
            var result = await _medicineService.GetMedicineByIdAsync(id);
            return HandleServiceResult(result, data => Ok(data));
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockMedicines()
        {
            var result = await _medicineService.GetLowStockMedicinesAsync();
            return HandleServiceResult(result, data => Ok(data));
        }

        [HttpGet("audit-required")]
        public async Task<IActionResult> GetMedicinesRequiringAudit()
        {
            var result = await _medicineService.GetMedicinesRequiringAuditAsync();
            return HandleServiceResult(result, data => Ok(data));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineDTO createMedicineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _medicineService.CreateMedicineAsync(createMedicineDTO);

            return HandleServiceResult(result, data =>
                CreatedAtAction(
                    nameof(GetMedicine),
                    new { id = data.Id },
                    data
                )
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMedicine(int id, [FromBody] MedicineDTO medicineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != medicineDTO.Id)
                return BadRequest("ID mismatch");

            var result = await _medicineService.UpdateMedicineAsync(id, medicineDTO);

            return HandleServiceResult(result, _ => NoContent());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var result = await _medicineService.DeleteMedicineAsync(id);

            return HandleServiceResult(result, _ => NoContent());
        }
    }
}
