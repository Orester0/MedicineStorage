using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Controllers
{
    public class MedicinesController(IUnitOfWork _unitOfWork, ILogger<MedicinesController> _logger) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAllMedicines()
        {
            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all medicines");
                return StatusCode(500, "Internal server error while retrieving medicines");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (medicine == null)
                    return NotFound($"Medicine with ID {id} not found");

                return Ok(medicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting medicine {MedicineId}", id);
                return StatusCode(500, "Internal server error while retrieving medicine");
            }
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetLowStockMedicines()
        {
            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetLowStockMedicinesAsync();
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting low stock medicines");
                return StatusCode(500, "Internal server error while retrieving low stock medicines");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Medicine>> CreateMedicine(Medicine medicine)
        {
            try
            {
                var success = await _unitOfWork.MedicineRepository.CreateAsync(medicine);
                if (!success)
                    return BadRequest("Failed to create medicine");

                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, medicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medicine");
                return StatusCode(500, "Internal server error while creating medicine");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, Medicine medicine)
        {
            if (id != medicine.Id)
                return BadRequest("ID mismatch");

            try
            {
                var success = await _unitOfWork.MedicineRepository.UpdateAsync(medicine);
                if (!success)
                    return NotFound($"Medicine with ID {id} not found");

                await _unitOfWork.Complete();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medicine {MedicineId}", id);
                return StatusCode(500, "Internal server error while updating medicine");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            try
            {
                var success = await _unitOfWork.MedicineRepository.DeleteAsync(id);
                if (!success)
                    return NotFound($"Medicine with ID {id} not found");

                await _unitOfWork.Complete();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medicine {MedicineId}", id);
                return StatusCode(500, "Internal server error while deleting medicine");
            }
        }
    }
}
