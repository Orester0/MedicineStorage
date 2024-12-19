using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicineStorage.Controllers.Implementation
{
    public class AuditController(IAuditService _auditService) : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAllAudits([FromQuery] AuditParams auditParams)
        {
            var result = await _auditService.GetAllAuditsAsync(auditParams);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("{auditId:int}")]
        public async Task<IActionResult> GetAuditById(int auditId)
        {
            var result = await _auditService.GetAuditByIdAsync(auditId);

            if (!result.Success)
            {
                return NotFound(result.Errors);
            }

            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("create")]
        public async Task<IActionResult> CreateAudit([FromBody] CreateAuditRequest request)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.CreateAuditAsync(userId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return CreatedAtAction(nameof(GetAuditById), new { auditId = result.Data.Id }, result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("start/{auditId:int}")]
        public async Task<IActionResult> StartAudit(int auditId, [FromBody] StartAuditRequest request)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.StartAuditAsync(userId, auditId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("update-items/{auditId:int}")]
        public async Task<IActionResult> UpdateAuditItems(int auditId, [FromBody] UpdateAuditItemsRequest request)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.UpdateAuditItemsAsync(userId, auditId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("close/{auditId:int}")]
        public async Task<IActionResult> CloseAudit(int auditId, [FromBody] CloseAuditRequest request)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.CloseAuditAsync(userId, auditId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("update/{auditId:int}")]
        public async Task<IActionResult> UpdateAudit(int auditId, [FromBody] Audit audit)
        {
            if (auditId != audit.Id)
            {
                return BadRequest("Audit ID mismatch");
            }

            var result = await _auditService.UpdateAuditAsync(audit);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return Ok(result.Data);
        }

        [HttpDelete("{auditId:int}")]
        public async Task<IActionResult> DeleteAudit(int auditId)
        {
            var result = await _auditService.DeleteAuditAsync(auditId);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return NoContent();
        }
    }

}