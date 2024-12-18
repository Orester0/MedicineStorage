using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicineStorage.Controllers.Implementation
{
    [Authorize]
    public class AuditController(IAuditService _auditService) : BaseApiController
    {
        [HttpPost("create")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> CreateAudit([FromBody] CreateAuditRequest request)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _auditService.CreateAuditAsync(userId, request);

                if (result.Errors.Any())
                {
                    return BadRequest(result.Errors);
                }

                return CreatedAtAction(nameof(GetAuditById), new { auditId = result.Data.Id }, result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("start")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> StartAudit([FromBody] StartAuditRequest request)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _auditService.StartAuditAsync(userId, request);

                if (result.Errors.Any())
                {
                    return BadRequest(result.Errors);
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("{auditId}/update-items")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> UpdateAuditItems(int auditId, [FromBody] UpdateAuditItemsRequest request)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _auditService.UpdateAuditItemsAsync(userId, request);

                if (result.Errors.Any())
                {
                    return BadRequest(result.Errors);
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpPut("{auditId}/close")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> CloseAudit(CloseAuditRequest request)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _auditService.CloseAuditAsync(userId, request);

                if (result.Errors.Any())
                {
                    return BadRequest(result.Errors);
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> GetAllAudits()
        {
            var result = await _auditService.GetAllAuditsAsync();

            if (result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpGet("{auditId}")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> GetAuditById(int auditId)
        {
            var result = await _auditService.GetAuditByIdAsync(auditId);

            if (result.Errors.Any())
            {
                return NotFound(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpPut("{auditId}/update")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> UpdateAudit(int auditId, [FromBody] Audit audit)
        {
            if (auditId != audit.Id)
            {
                return BadRequest("Audit ID mismatch");
            }

            var result = await _auditService.UpdateAuditAsync(audit);

            if (result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{auditId}")]
        [Authorize(Roles = "Admin,SupremeAdmin")]
        public async Task<IActionResult> DeleteAudit(int auditId)
        {
            var result = await _auditService.DeleteAuditAsync(auditId);

            if (result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

    }

}