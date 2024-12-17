using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{
    public class TenderController(ITenderService _tenderService) : BaseApiController
    {
        // Admin and SupremeAdmin Actions

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateTender([FromBody] CreateTenderDTO tenderDto)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _tenderService.CreateTenderAsync(tenderDto, userId);
                if (!result.Success) return BadRequest(result.Errors);
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTender(int id, [FromBody] ReturnTenderDTO tenderDto)
        {
            tenderDto.Id = id;
            var result = await _tenderService.UpdateTenderAsync(tenderDto);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpPost("{id}/publish")]
        public async Task<IActionResult> PublishTender(int id)
        {
            var result = await _tenderService.PublishTenderAsync(id);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseTender(int id)
        {
            var result = await _tenderService.CloseTenderAsync(id);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllTenders()
        {
            var result = await _tenderService.GetAllTendersAsync();
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenderById(int id)
        {
            var result = await _tenderService.GetTenderByIdAsync(id);
            if (!result.Success) return NotFound(result.Errors);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Admin,SupremeAdmin")]
        [HttpPost("{id}/select-proposal")]
        public async Task<IActionResult> SelectWinningProposal(int id, [FromBody] int proposalId)
        {
            var result = await _tenderService.SelectWinningProposalAsync(proposalId);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        // Distributor Actions

        [Authorize(Roles = "Distributor")]
        [HttpGet("published")]
        public async Task<IActionResult> GetPublishedTenders()
        {
            var result = await _tenderService.GetTendersByStatusAsync(TenderStatus.Published);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [Authorize(Roles = "Distributor")]
        [HttpPost("{id}/proposals")]
        public async Task<IActionResult> SubmitProposal(int id, [FromBody] CreateTenderProposalDTO proposalDto)
        {
            try
            {
                proposalDto.TenderId = id;
                var userId = GetUserIdFromClaims();
                var result = await _tenderService.SubmitProposalAsync(proposalDto, userId);
                if (!result.Success) return BadRequest(result.Errors);
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Distributor")]
        [HttpPut("proposals/{id}")]
        public async Task<IActionResult> UpdateProposal(int id, [FromBody] ReturnTenderProposalDTO proposalDto)
        {
            try
            {
                proposalDto.Id = id;
                var userId = GetUserIdFromClaims();
                var result = await _tenderService.UpdateProposalAsync(proposalDto, userId);
                if (!result.Success) return BadRequest(result.Errors);
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Distributor")]
        [HttpDelete("proposals/{id}")]
        public async Task<IActionResult> WithdrawProposal(int id)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _tenderService.WithdrawProposalAsync(id, userId);
                if (!result.Success) return BadRequest(result.Errors);
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Distributor")]
        [HttpGet("proposals")]
        public async Task<IActionResult> GetUserProposals()
        {
            try
            {
                var userId = GetUserIdFromClaims();
                var result = await _tenderService.GetTendersByUserAsync(userId);
                if (!result.Success) return BadRequest(result.Errors);
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
