using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.Tender;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface ITenderService
    {
        Task<ServiceResult<PagedList<Tender>>> GetAllTendersAsync(TenderParams tenderParams);
        Task<ServiceResult<ReturnTenderDTO>> GetTenderByIdAsync(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersCreatedByUserId(int userId);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersAwardedByUserId(int userId);
        Task<ServiceResult<IEnumerable<ReturnTenderItemDTO>>> GetItemsByTenderId(int tenderId);

        Task<ServiceResult<ReturnTenderProposalDTO>> GetProposalByIdAsync(int proposalId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsByTenderId(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>> GetItemsByProposalId(int proposalId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsCreatedByUserId(int userId);






        Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId);
        Task<ServiceResult<ReturnTenderItemDTO>> AddTenderItemAsync(CreateTenderItemDTO tenderItemDto);
        Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(CreateTenderProposalDTO proposalDto, List<CreateTenderProposalItemDTO> proposalItemsDto, int userId);
        Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId, int userId);

        Task<ServiceResult<bool>> ExecuteTenderItemAsync(int tenderItemId, int proposalId, int userId);

        Task<ServiceResult<bool>> ExecuteTenderAsync(int proposalId, int userId);


    }
}

