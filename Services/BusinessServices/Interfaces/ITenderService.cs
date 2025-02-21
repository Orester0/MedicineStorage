using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface ITenderService
    {

        Task<ServiceResult<List<ReturnTenderDTO>>> GetAllTendersAsync();
        Task<ServiceResult<PagedList<ReturnTenderDTO>>> GetPaginatedTenders(TenderParams tenderParams);
        Task<ServiceResult<ReturnTenderDTO>> GetTenderByIdAsync(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersCreatedByUserId(int userId);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersAwardedByUserId(int userId);

        Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsByTenderId(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsCreatedByUserId(int userId);




        Task<ServiceResult<bool>> DeleteTenderAsync(int requestId, int userId, List<string> userRoles);


        Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId);
        Task<ServiceResult<ReturnTenderItemDTO>> AddTenderItemAsync(int tenderId, CreateTenderItemDTO tenderItemDto);
        Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(int tenderId, CreateTenderProposalDTO proposalDto, int userId);
        Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId, int userId);

        Task<ServiceResult<bool>> ExecuteTenderItemAsync(int tenderItemId, int proposalId, int userId);

        Task<ServiceResult<bool>> ExecuteTenderAsync(int proposalId, int userId);


    }
}

