using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models.TenderModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{

    public class DistributorController(IUnitOfWork _unitOfWork, ILogger<DistributorController> _logger, IMapper _mapper) : BaseApiController
    {

    //    [HttpGet("tenders")]
    //    public async Task<IActionResult> GetActiveTenders()
    //    {
    //        try
    //        {
    //            var tenders = await _unitOfWork.TenderRepository.GetActiveTendersAsync();
    //            var tenderDTOs = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
    //            return Ok(tenderDTOs);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error retrieving active tenders.");
    //            return StatusCode(500, "Internal server error");
    //        }
    //    }

    //    [HttpDelete("proposal-items/{itemId}")]
    //    public async Task<IActionResult> RemoveProposalItem(int itemId)
    //    {
    //        try
    //        {
    //            bool result = await _unitOfWork.TenderProposalRepository.RemoveProposalItemAsync(itemId);
    //            if (result)
    //            {
    //                return NoContent();
    //            }
    //            return BadRequest("Failed to remove proposal item.");
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error removing proposal item.");
    //            return StatusCode(500, "Internal server error");
    //        }
    //    }


    //    [HttpDelete("proposals/{proposalId}")]
    //    public async Task<IActionResult> DeleteProposal(int proposalId)
    //    {
    //        try
    //        {
    //            bool result = await _unitOfWork.TenderProposalRepository.DeleteProposalAsync(proposalId);
    //            if (result)
    //            {
    //                return NoContent();
    //            }
    //            return BadRequest("Failed to delete proposal.");
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error deleting proposal.");
    //            return StatusCode(500, "Internal server error");
    //        }
    //    }
    }
}


//[HttpGet("proposals/{distributorId}")]
//public async Task<IActionResult> GetProposalsByDistributor(int distributorId)
//{
//    try
//    {
//        var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByDistributorAsync(distributorId);
//        var proposalDtos = proposals.Select(p => new TenderProposalDTO
//        {
//            Id = p.Id,
//            TenderId = p.TenderId,
//            DistributorId = p.DistributorId,
//            TotalPrice = p.TotalPrice,
//            SubmissionDate = p.SubmissionDate,
//            Status = p.Status,
//            Items = p.Items.Select(i => new TenderProposalItemDTO
//            {
//                Id = i.Id,
//                TenderProposalId = i.TenderProposalId,
//                MedicineId = i.MedicineId,
//                UnitPrice = i.UnitPrice,
//                Quantity = i.Quantity
//            }).ToList()
//        }).ToList();
//        return Ok(proposalDtos);
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "Error retrieving proposals for distributor.");
//        return StatusCode(500, "Internal server error");
//    }
//}

//    [HttpPost("proposals")]
//    public async Task<IActionResult> SubmitProposal([FromBody] TenderProposalDTO proposalDto)
//    {
//        try
//        {
//            if (proposalDto == null || proposalDto.Items == null || proposalDto.Items.Count == 0)
//            {
//                return BadRequest("Proposal must contain at least one item.");
//            }

//            var proposal = new TenderProposal
//            {
//                TenderId = proposalDto.TenderId,
//                DistributorId = proposalDto.DistributorId,
//                TotalPrice = proposalDto.TotalPrice,
//                SubmissionDate = proposalDto.SubmissionDate,
//                Status = proposalDto.Status,
//                Items = proposalDto.Items.Select(i => new TenderProposalItem
//                {
//                    TenderProposalId = i.TenderProposalId,
//                    MedicineId = i.MedicineId,
//                    UnitPrice = i.UnitPrice,
//                    Quantity = i.Quantity
//                }).ToList()
//            };

//            bool result = await _unitOfWork.TenderProposalRepository.SubmitProposalAsync(proposal);
//            if (result)
//            {
//                return CreatedAtAction(nameof(GetProposalsByDistributor), new { distributorId = proposal.DistributorId }, proposalDto);
//            }
//            return BadRequest("Failed to submit proposal.");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error submitting proposal.");
//            return StatusCode(500, "Internal server error");
//        }
//    }

//    [HttpPost("proposal-items")]
//    public async Task<IActionResult> AddProposalItem([FromBody] TenderProposalItemDTO itemDto)
//    {
//        try
//        {
//            if (itemDto == null)
//            {
//                return BadRequest("Proposal item data is required.");
//            }

//            var item = new TenderProposalItem
//            {
//                TenderProposalId = itemDto.TenderProposalId,
//                MedicineId = itemDto.MedicineId,
//                UnitPrice = itemDto.UnitPrice,
//                Quantity = itemDto.Quantity
//            };

//            bool result = await _unitOfWork.TenderProposalRepository.AddProposalItemAsync(item);
//            if (result)
//            {
//                return Ok(new TenderProposalItemDTO
//                {
//                    Id = item.Id,
//                    TenderProposalId = item.TenderProposalId,
//                    MedicineId = item.MedicineId,
//                    UnitPrice = item.UnitPrice,
//                    Quantity = item.Quantity
//                });
//            }
//            return BadRequest("Failed to add proposal item.");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error adding proposal item.");
//            return StatusCode(500, "Internal server error");
//        }
//    }

//    [HttpPut("proposal-items/{itemId}")]
//    public async Task<IActionResult> UpdateProposalItem(int itemId, [FromBody] TenderProposalItemDTO itemDto)
//    {
//        try
//        {
//            if (itemDto == null || itemDto.Id != itemId)
//            {
//                return BadRequest("Mismatched item data.");
//            }

//            var item = new TenderProposalItem
//            {
//                Id = itemDto.Id,
//                TenderProposalId = itemDto.TenderProposalId,
//                MedicineId = itemDto.MedicineId,
//                UnitPrice = itemDto.UnitPrice,
//                Quantity = itemDto.Quantity
//            };

//            bool result = await _unitOfWork.TenderProposalRepository.UpdateProposalItemAsync(item);
//            if (result)
//            {
//                return NoContent();
//            }
//            return BadRequest("Failed to update proposal item.");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error updating proposal item.");
//            return StatusCode(500, "Internal server error");
//        }
//    }


//    [HttpPut("proposals/{proposalId}/status")]
//    public async Task<IActionResult> UpdateProposalStatus(int proposalId, [FromBody] ProposalStatus status)
//    {
//        try
//        {
//            bool result = await _unitOfWork.TenderProposalRepository.UpdateProposalStatusAsync(proposalId, status);
//            if (result)
//            {
//                return NoContent();
//            }
//            return BadRequest("Failed to update proposal status.");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error updating proposal status.");
//            return StatusCode(500, "Internal server error");
//        }
//    }