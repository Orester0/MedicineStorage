using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models;
using MedicineStorage.Services.Implementations;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{
    public class MedicinesController(IMedicineService _medicineService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetMedicines([FromQuery] MedicineParams parameters)
        {
            var result = await _medicineService.GetMedicinesAsync(parameters);
            return result.Success
                ? Ok(result.Data)
                : StatusCode(500, result.Errors);
        }

        [HttpGet("{medicineId:int}")]
        public async Task<IActionResult> GetMedicineById(int medicineId)
        {
            var result = await _medicineService.GetMedicineByIdAsync(medicineId);
            return result.Success
                ? Ok(result.Data)
                : StatusCode(404, result.Errors);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineDTO createMedicineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _medicineService.CreateMedicineAsync(createMedicineDTO);
            return result.Success
                ? CreatedAtAction(nameof(GetMedicineById), new { id = result.Data.Id }, result.Data)
                : StatusCode(400, result.Errors);
        }

        [HttpPut("{medicineId:int}")]
        public async Task<IActionResult> UpdateMedicine(int medicineId, [FromBody] CreateMedicineDTO medicineDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var result = await _medicineService.UpdateMedicineAsync(medicineId, medicineDTO);
            return result.Success
                ? NoContent()
                : StatusCode(400, result.Errors);
        }

        [HttpDelete("{medicineId:int}")]
        public async Task<IActionResult> DeleteMedicine(int medicineId)
        {
            var result = await _medicineService.DeleteMedicineAsync(medicineId);
            return result.Success
                ? NoContent()
                : StatusCode(400, result.Errors);
        }
    }
}
