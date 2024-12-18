using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface ITenderService
    {
        Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId);
        Task<ServiceResult<ReturnTenderItemDTO>> AddTenderItemAsync(CreateTenderItemDTO tenderItemDto);
        Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(CreateTenderProposalDTO proposalDto, List<CreateTenderProposalItemDTO> proposalItemsDto, int userId);
        Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId, int userId);

        // GET methods
        Task<ServiceResult<ReturnTenderDTO>> GetTenderByIdAsync(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersByStatusAsync(TenderStatus status);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersByUserAsync(int userId);
        Task<ServiceResult<IEnumerable<ReturnTenderItemDTO>>> GetTenderItemsAsync(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsForTenderAsync(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>> GetTenderProposalItemsAsync(int proposalId);
    }
}

