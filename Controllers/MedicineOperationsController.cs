using AutoMapper;
using MedicineStorage.Data;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Claims;

namespace MedicineStorage.Controllers
{
    public class MedicineOperationsController(IUnitOfWork _unitOfWork, ILogger<MedicineOperationsController> _logger, IMapper _mapper) : BaseApiController
    {
        [HttpPost("request")]
        public async Task<ActionResult<MedicineRequestDTO>> CreateMedicineRequest(CreateMedicineRequestDTO createRequestDto)
        {
            try
            {
                var request = _mapper.Map<MedicineRequest>(createRequestDto);
                

                var success = await _unitOfWork.MedicineRequestRepository.CreateRequestAsync(request);
                if (!success)
                    return BadRequest("Failed to create medicine request");

                await _unitOfWork.Complete();

                var createdRequest = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(request.Id);
                var responseDto = _mapper.Map<MedicineRequestDTO>(createdRequest);

                return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medicine request");
                return StatusCode(500, "Internal server error while creating request");
            }
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetAllRequests()
        {
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetAllAsync();
                var requestDtos = _mapper.Map<IEnumerable<MedicineRequestDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all requests");
                return StatusCode(500, "Internal server error while retrieving requests");
            }
        }

        [HttpGet("requests/pending")]
        public async Task<ActionResult<IEnumerable<MedicineRequestDTO>>> GetPendingRequests()
        {
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetPendingRequestsAsync();
                var requestDtos = _mapper.Map<IEnumerable<MedicineRequestDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending requests");
                return StatusCode(500, "Internal server error while retrieving pending requests");
            }
        }

        [HttpGet("requests/{id}")]
        public async Task<ActionResult<MedicineRequestDTO>> GetRequest(int id)
        {
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(id);
                if (request == null)
                    return NotFound($"Request with ID {id} not found");

                var requestDto = _mapper.Map<MedicineRequestDTO>(request);
                return Ok(requestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting request {RequestId}", id);
                return StatusCode(500, "Internal server error while retrieving request");
            }
        }

        [HttpPut("requests/{id}/status")]
        public async Task<IActionResult> UpdateRequestStatus(int id, RequestStatus status)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var success = await _unitOfWork.MedicineRequestRepository.UpdateRequestStatusAsync(
                    id,
                    status,
                    status == RequestStatus.Approved ? userId : null
                );

                if (!success)
                    return NotFound($"Request with ID {id} not found");

                await _unitOfWork.Complete();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating request status {RequestId}", id);
                return StatusCode(500, "Internal server error while updating request status");
            }
        }

        [HttpPost("usage")]
        public async Task<ActionResult<MedicineUsageDTO>> RecordMedicineUsage(CreateMedicineUsageDTO createUsageDto)
        {
            try
            {
                var usage = _mapper.Map<MedicineUsage>(createUsageDto);
                

                var success = await _unitOfWork.MedicineUsageRepository.RecordUsageAsync(usage);
                if (!success)
                    return BadRequest("Failed to record medicine usage. Please check stock availability.");

                await _unitOfWork.Complete();

                // Get the created usage with related data
                var createdUsage = await _unitOfWork.MedicineUsageRepository.GetByIdAsync(usage.Id);
                var responseDto = _mapper.Map<MedicineUsageDTO>(createdUsage);

                return CreatedAtAction(nameof(GetUsage), new { id = usage.Id }, responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording medicine usage");
                return StatusCode(500, "Internal server error while recording usage");
            }
        }

        [HttpGet("usage/{id}")]
        public async Task<ActionResult<MedicineUsageDTO>> GetUsage(int id)
        {
            try
            {
                var usage = await _unitOfWork.MedicineUsageRepository.GetByIdAsync(id);
                if (usage == null)
                    return NotFound($"Usage record with ID {id} not found");

                var usageDto = _mapper.Map<MedicineUsageDTO>(usage);
                return Ok(usageDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting usage {UsageId}", id);
                return StatusCode(500, "Internal server error while retrieving usage record");
            }
        }

        [HttpGet("usage/medicine/{medicineId}")]
        public async Task<ActionResult<IEnumerable<MedicineUsageDTO>>> GetUsageByMedicine(int medicineId)
        {
            try
            {
                var usages = await _unitOfWork.MedicineUsageRepository.GetUsagesByMedicineAsync(medicineId);
                var usageDtos = _mapper.Map<IEnumerable<MedicineUsageDTO>>(usages);
                return Ok(usageDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting usage for medicine {MedicineId}", medicineId);
                return StatusCode(500, "Internal server error while retrieving usage records");
            }
        }

        [HttpGet("usage/user/{userId}")]
        public async Task<ActionResult<IEnumerable<MedicineUsageDTO>>> GetUsageByUser(int userId)
        {
            try
            {
                var usages = await _unitOfWork.MedicineUsageRepository.GetUsagesByUserAsync(userId);
                var usageDtos = _mapper.Map<IEnumerable<MedicineUsageDTO>>(usages);
                return Ok(usageDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting usage for user {UserId}", userId);
                return StatusCode(500, "Internal server error while retrieving usage records");
            }
        }

        [HttpGet("usage/date-range")]
        public async Task<ActionResult<IEnumerable<MedicineUsageDTO>>> GetUsageByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var usages = await _unitOfWork.MedicineUsageRepository.GetUsagesByDateRangeAsync(startDate, endDate);
                var usageDtos = _mapper.Map<IEnumerable<MedicineUsageDTO>>(usages);
                return Ok(usageDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting usage for date range");
                return StatusCode(500, "Internal server error while retrieving usage records");
            }
        }

    }
}
