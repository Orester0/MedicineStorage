using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Controllers
{
    public class MedicinesController(IUnitOfWork _unitOfWork, ILogger<MedicinesController> _logger, IMapper _mapper) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetAllMedicines()
        {
            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
                var medicineDTOs = _mapper.Map<IEnumerable<MedicineDTO>>(medicines);
                return Ok(medicineDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all medicines");
                return StatusCode(500, "Internal server error while retrieving medicines");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDTO>> GetMedicine(int id)
        {
            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (medicine == null)
                    return NotFound($"Medicine with ID {id} not found");

                var medicineDTO = _mapper.Map<MedicineDTO>(medicine);
                return Ok(medicineDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting medicine {MedicineId}", id);
                return StatusCode(500, "Internal server error while retrieving medicine");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicineDTO>> CreateMedicine(CreateMedicineDTO createMedicineDTO)
        {
            try
            {
                var medicine = _mapper.Map<Medicine>(createMedicineDTO);

                var success = await _unitOfWork.MedicineRepository.CreateAsync(medicine);
                if (!success)
                    return BadRequest("Failed to create medicine");

                await _unitOfWork.Complete();

                var createdMedicineDTO = _mapper.Map<MedicineDTO>(medicine);

                return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, createdMedicineDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medicine");
                return StatusCode(500, "Internal server error while creating medicine");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, MedicineDTO medicineDTO)
        {
            if (id != medicineDTO.Id)
                return BadRequest("ID mismatch");

            try
            {
                var medicine = _mapper.Map<Medicine>(medicineDTO);

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
