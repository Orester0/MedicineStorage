using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.BusinessServices.Implementations;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    public class TenderController(ITenderService _tenderService) : BaseApiController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTenders()
        {
            var result = await _tenderService.GetAllTendersAsync();
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenders([FromQuery] TenderParams tenderParams)
        {
            var result = await _tenderService.GetPaginatedTenders(tenderParams);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("created-by/{userId:int}")]
        public async Task<IActionResult> GetTendersCreatedByUser(int userId)
        {
            
                var result = await _tenderService.GetTendersCreatedByUserId(userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpGet("awarded-by/{userId:int}")]
        public async Task<IActionResult> GetTendersAwardedByUser(int userId)
        {
            var result = await _tenderService.GetTendersAwardedByUserId(userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("{tenderId:int}")]
        public async Task<IActionResult> GetTenderById(int tenderId)
        {
            var result = await _tenderService.GetTenderByIdAsync(tenderId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpGet("{tenderId:int}/proposals")]
        public async Task<IActionResult> GetProposalsByTenderId(int tenderId)
        {
            var result = await _tenderService.GetProposalsByTenderId(tenderId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("create")]
        public async Task<IActionResult> CreateTender([FromBody] CreateTenderDTO tenderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userId = User.GetUserIdFromClaims();
            var result = await _tenderService.CreateTenderAsync(tenderDto, userId);


            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
            
        }

        [HttpPost("add-tender-item/{tenderId:int}")]
        public async Task<IActionResult> AddTenderItemToTender(int tenderId, [FromBody] CreateTenderItemDTO tenderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _tenderService.AddTenderItemAsync(tenderId, tenderDto);

            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

        [HttpPut("publish/{tenderId:int}")]
        public async Task<IActionResult> PublishTender(int tenderId)
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.PublishTenderAsync(tenderId, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpPost("proposals/submit/{proposalId:int}")]
        public async Task<IActionResult> SubmitProposal(int proposalId, [FromBody] CreateTenderProposalDTO proposalDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserIdFromClaims();
            var result = await _tenderService.SubmitProposalAsync(proposalId, proposalDto, userId);
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);
        }

        [HttpPut("proposals/execute/{proposalId:int}/{tenderItemId:int}")]
        public async Task<IActionResult> ExecuteTenderItem(int tenderItemId, int proposalId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _tenderService.ExecuteTenderItemAsync(tenderItemId, proposalId, userId);
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);
        }

        [HttpPut("proposals/execute/{proposalId:int}")]
        public async Task<IActionResult> ExecuteTender(int proposalId)
        {
            var userId = User.GetUserIdFromClaims();
            var result = await _tenderService.ExecuteTenderAsync(proposalId, userId);
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);
        }

        [HttpPut("close/{tenderId:int}")]
        public async Task<IActionResult> CloseTender(int tenderId)
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.CloseTenderAsync(tenderId, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpPut("{tenderId:int}/select-winning-proposal/{proposalId:int}")]
        public async Task<IActionResult> SelectWinningProposal(int tenderId, int proposalId)
        {
            
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.SelectWinningProposalAsync(proposalId, userId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Errors });
                }
                return Ok(result.Data);
            
        }

        [HttpDelete("{tenderId:int}")]
        public async Task<IActionResult> DeleteTender(int tenderId)
        {

            var userId = User.GetUserIdFromClaims();
            var roles = User.GetUserRolesFromClaims();
            var result = await _tenderService.DeleteTenderAsync(tenderId, userId, roles);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }
    }
}
