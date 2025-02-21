using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    public class AuditController(IAuditService _auditService) : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAllAudits([FromQuery] AuditParams auditParams)
        {
            var result = await _auditService.GetPaginatedAudits(auditParams);
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
                return BadRequest(new { result.Errors });
                
            }

            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("create")]
        public async Task<IActionResult> CreateAudit([FromBody] CreateAuditDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.CreateAuditAsync(userId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return CreatedAtAction(nameof(GetAuditById), new { auditId = result.Data.Id }, result.Data);

        }

        [HttpPut("start/{auditId:int}")]
        public async Task<IActionResult> StartAudit(int auditId, [FromBody] CreateAuditNoteDTO request)
        {
            
                var userId = User.GetUserIdFromClaims();
                var userRoles = User.GetUserRolesFromClaims();
                var result = await _auditService.StartAuditAsync(userId, auditId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return Ok(result.Data);
            
        }

        [HttpPut("update-items/{auditId:int}")]
        public async Task<IActionResult> UpdateAuditItems(int auditId, [FromBody] UpdateAuditItemsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.UpdateAuditItemsAsync(userId, auditId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return Ok(result.Data);
            
        }

        [HttpPut("close/{auditId:int}")]
        public async Task<IActionResult> CloseAudit(int auditId, [FromBody] CreateAuditNoteDTO request)
        {
                var userId = User.GetUserIdFromClaims();
                var result = await _auditService.CloseAuditAsync(userId, auditId, request);

                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }

                return Ok(result.Data);
            
        }


        [HttpDelete("{auditId:int}")]
        public async Task<IActionResult> DeleteAudit(int auditId)
        {

            var userId = User.GetUserIdFromClaims();
            var roles = User.GetUserRolesFromClaims();
            var result = await _auditService.DeleteAuditAsync(auditId, userId, roles);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }

            return NoContent();
        }
    }

}