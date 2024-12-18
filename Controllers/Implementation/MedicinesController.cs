using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Services.Implementations;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{
    //[Authorize(Policy = "Admin")]
    public class MedicinesController(IMedicineService _medicineService, ILogger<MedicinesController> _logger) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllMedicines()
        {
            var result = await _medicineService.GetAllMedicinesAsync();
            return result.Success
                ? Ok(result.Data)
                : StatusCode(500, result.Errors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMedicine(int id)
        {
            var result = await _medicineService.GetMedicineByIdAsync(id);
            return result.Success
                ? Ok(result.Data)
                : StatusCode(404, result.Errors);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockMedicines()
        {
            var result = await _medicineService.GetLowStockMedicinesAsync();
            return result.Success
                ? Ok(result.Data)
                : StatusCode(404, result.Errors);
        }

        [HttpGet("audit-required")]
        public async Task<IActionResult> GetMedicinesRequiringAudit()
        {
            var result = await _medicineService.GetMedicinesRequiringAuditAsync();
            return result.Success
                ? Ok(result.Data)
                : StatusCode(404, result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineDTO createMedicineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _medicineService.CreateMedicineAsync(createMedicineDTO);
            return result.Success
                ? CreatedAtAction(nameof(GetMedicine), new { id = result.Data.Id }, result.Data)
                : StatusCode(400, result.Errors);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMedicine(int id, [FromBody] CreateMedicineDTO medicineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var result = await _medicineService.UpdateMedicineAsync(id, medicineDTO);
            return result.Success
                ? NoContent()
                : StatusCode(400, result.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var result = await _medicineService.DeleteMedicineAsync(id);
            return result.Success
                ? NoContent()
                : StatusCode(400, result.Errors);
        }
    }
}
