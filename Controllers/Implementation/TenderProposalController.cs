using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{
    public class TenderProposalController(ITenderService _tenderService) : BaseApiController
    {
        [HttpGet("{proposalId:int}")]
        public async Task<IActionResult> GetProposalById(int proposalId)
        {
            var result = await _tenderService.GetProposalByIdAsync(proposalId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("{proposalId:int}/proposal-items")]
        public async Task<IActionResult> GetProposalItemsByProposal(int proposalId)
        {
            var result = await _tenderService.GetItemsByProposalId(proposalId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("submit/{proposalId:int}")]
        public async Task<IActionResult> SubmitProposal(int proposalId, [FromBody] CreateTenderProposalDTO proposalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.SubmitProposalAsync(proposalId, proposalDto, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpPut("execute/{proposalId:int}/{tenderItemId:int}")]
        public async Task<IActionResult> ExecuteTenderItem(int tenderItemId, int proposalId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.ExecuteTenderItemAsync(tenderItemId, proposalId, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpPut("execute/{proposalId:int}")]
        public async Task<IActionResult> ExecuteTender(int proposalId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.ExecuteTenderAsync(proposalId, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }
    }
}
