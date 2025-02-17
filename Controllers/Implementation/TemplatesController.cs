using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    public class TemplatesController(ITemplateService _notificationTemplateService) : BaseApiController
    {
        [HttpGet("tender-templates")]
        public async Task<IActionResult> GetAllTenderTemplatesByUserIdAsync()
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.GetTenderTemplatesByUserIdAsync(userId);
            return Ok(result.Data);
        }

        [HttpGet("audit-templates")]
        public async Task<IActionResult> GetAuditTemplatesByUserIdAsync()
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.GetAuditTemplatesByUserIdAsync(userId);
            return Ok(result.Data);
        }


        [HttpGet("medicine-request-templates")]
        public async Task<IActionResult> GetMedicineRequestTemplatesByUserIdAsync()
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.GetMedicineRequestTemplatesByUserIdAsync(userId);
            return Ok(result.Data);
        }


        //-------------------------------------------------------------------------------------------------------------------------
        
        [HttpPost("medicine-request-templates")]
        public async Task<IActionResult> CreateMedicineRequestTemplateAsync([FromBody] MedicineRequestTemplateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.CreateMedicineRequestTemplateAsync(dto, userId);
            return Ok(result.Data);
        }

        [HttpPost("audit-templates")]
        public async Task<IActionResult> CreateAuditTemplateAsync([FromBody] AuditTemplateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.CreateAuditTemplateAsync(dto, userId);
            return Ok(result.Data);
        }

        [HttpPost("tender-templates")]
        public async Task<IActionResult> CreateTenderTemplateAsync([FromBody] TenderTemplateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.CreateTenderTemplateAsync(dto, userId);
            return Ok(result.Data);
        }

        //------------------------------------------------------------------------------------------------------------------------

        [HttpPut("medicine-request-templates/{templateId:int}")]
        public async Task<IActionResult> UpdateMedicineRequestTemplateAsync(int templateId, [FromBody] MedicineRequestTemplateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.UpdateMedicineRequestTemplateAsync(templateId, dto, userId);
            return Ok(result.Data);
        }

        [HttpPut("audit-templates/{templateId:int}")]
        public async Task<IActionResult> UpdateAuditTemplateAsync(int templateId, [FromBody] AuditTemplateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.UpdateAuditTemplateAsync(templateId, dto, userId);
            return Ok(result.Data);
        }

        [HttpPut("tender-templates/{templateId:int}")]
        public async Task<IActionResult> UpdateTenderTemplateAsync(int templateId, [FromBody] TenderTemplateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.UpdateTenderTemplateAsync(templateId, dto, userId);
            return Ok(result.Data);
        }

        //----------------------------------------------------------------------------------------------------------------

        [HttpPost("medicine-request-templates/execute/{templateId:int}")]
        public async Task<IActionResult> ExecuteMedicineRequestTemplateAsync(int templateId, [FromBody] DateTime dateTime)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.ExecuteMedicineRequestTemplateAsync(templateId, userId, dateTime);
            return Ok(result.Data);
        }

        [HttpPost("audit-templates/execute/{templateId:int}")]
        public async Task<IActionResult> ExecuteAuditTemplateAsync(int templateId, [FromBody] DateTime dateTime)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.ExecuteAuditTemplateAsync(templateId, userId, dateTime);
            return Ok(result.Data);
        }

        [HttpPost("tender-templates/execute/{templateId:int}")]
        public async Task<IActionResult> ExecuteTenderTemplateAsync(int templateId, [FromBody] DateTime dateTime)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.ExecuteTenderTemplateAsync(templateId, userId, dateTime);
            return Ok(result.Data);
        }

        //--------------------------------------------------------------------------------------------------------------

        [HttpPut("medicine-request-templates/deactivate/{templateId:int}")]
        public async Task<IActionResult> DeactivateMedicineRequestTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.DeactivateMedicineRequestTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        [HttpPut("audit-templates/deactivate/{templateId:int}")]
        public async Task<IActionResult> DeactivateAuditTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.DeactivateAuditTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        [HttpPut("tender-templates/deactivate/{templateId:int}")]
        public async Task<IActionResult> DeactivateTenderTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.DeactivateTenderTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        //---------------------------------------------------------------------------------------------------------

        [HttpPut("medicine-request-templates/activate/{templateId:int}")]
        public async Task<IActionResult> ActivateMedicineRequestTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.ActivateMedicineRequestTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        [HttpPut("audit-templates/activate/{templateId:int}")]
        public async Task<IActionResult> ActivateAuditTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.ActivateAuditTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        [HttpPut("tender-templates/activate/{templateId:int}")]
        public async Task<IActionResult> ActivateTenderTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.ActivateTenderTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        //---------------------------------------------------------------------------------------------------------

        [HttpDelete("medicine-request-templates/{templateId:int}")]
        public async Task<IActionResult> DeleteMedicineRequestTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.DeleteMedicineRequestTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        [HttpDelete("audit-templates/{templateId:int}")]
        public async Task<IActionResult> DeleteAuditTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.DeleteAuditTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

        [HttpDelete("tender-templates/{templateId:int}")]
        public async Task<IActionResult> DeleteTenderTemplateAsync(int templateId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _notificationTemplateService.DeleteTenderTemplateAsync(templateId, userId);
            return Ok(result.Data);
        }

    }
}
