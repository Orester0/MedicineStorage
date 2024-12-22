using MedicineStorage.Controllers.Interface;
using MedicineStorage.DTOs;
using MedicineStorage.Extensions;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{
    public class TenderController(ITenderService _tenderService) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetTenders([FromQuery] TenderParams tenderParams)
        {
            var result = await _tenderService.GetAllTendersAsync(tenderParams);

            if (!result.Success)
                return BadRequest(new { result.Errors });

            return Ok(new { result.Data } );
        }

        [HttpGet("created-by/{userId:int}")]
        public async Task<IActionResult> GetTendersCreatedByUser(int userId)
        {
            try
            {
                var result = await _tenderService.GetTendersCreatedByUserId(userId);
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(new { result.Data } );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpGet("awarded-by/{userId:int}")]
        public async Task<IActionResult> GetTendersAwardedByUser(int userId)
        {
            try
            {
                var result = await _tenderService.GetTendersAwardedByUserId(userId);
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(new { result.Data } );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpGet("{tenderId:int}")]
        public async Task<IActionResult> GetTenderById(int tenderId)
        {
            var result = await _tenderService.GetTenderByIdAsync(tenderId);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(new { result.Data } );
        }

        [HttpGet("{tenderId:int}/tender-items")]
        public async Task<IActionResult> GetTenderItemsByTender(int tenderId)
        {
            var result = await _tenderService.GetItemsByTenderId(tenderId);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(new { result.Data } );
        }

        [HttpGet("{tenderId:int}/proposals")]
        public async Task<IActionResult> GetProposalsByTenderId(int tenderId)
        {
            var result = await _tenderService.GetProposalsByTenderId(tenderId);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(new { result.Data } );
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost("create")]
        public async Task<IActionResult> CreateTender([FromBody] CreateTenderDTO tenderDto)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.CreateTenderAsync(tenderDto, userId);


                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(new { result.Data } );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpPut("publish/{tenderId:int}")]
        public async Task<IActionResult> PublishTender(int tenderId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.PublishTenderAsync(tenderId, userId);
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(new { result.Data } );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpPut("close/{tenderId:int}")]
        public async Task<IActionResult> CloseTender(int tenderId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.CloseTenderAsync(tenderId, userId);
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(new { result.Data } );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

        [HttpPut("{tenderId:int}/select-winning-proposal/{proposalId:int}")]
        public async Task<IActionResult> SelectWinningProposal(int tenderId, int proposalId)
        {
            try
            {
                var userId = User.GetUserIdFromClaims();
                var result = await _tenderService.SelectWinningProposalAsync(proposalId, userId);
                if (!result.Success) return BadRequest(new { result.Errors });
                return Ok(new { result.Data } );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Errors = new [] { ex.Message } });
            }
        }

    }
}
