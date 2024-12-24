using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{
    public class MedicinesController(IMedicineService _medicineService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetMedicines([FromQuery] MedicineParams parameters)
        {
            var result = await _medicineService.GetMedicinesAsync(parameters);

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
        public async Task<IActionResult> UpdateMedicine(int medicineId, [FromBody] CreateMedicineDTO medicineDTO)
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
            var result = await _medicineService.DeleteMedicineAsync(medicineId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }
    }
}
