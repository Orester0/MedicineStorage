using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface ITenderService
    {
        Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId);
        Task<ServiceResult<ReturnTenderDTO>> UpdateTenderAsync(ReturnTenderDTO tenderDto);
        Task<ServiceResult<bool>> DeleteTenderAsync(int tenderId);
        Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId);
        Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId);
        Task<ServiceResult<ReturnTenderDTO>> GetTenderByIdAsync(int tenderId);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetAllTendersAsync();
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersByStatusAsync(TenderStatus status);
        Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersByUserAsync(int userId);

        Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(CreateTenderProposalDTO proposalDto, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> UpdateProposalAsync(ReturnTenderProposalDTO proposalDto, int userId);
        Task<ServiceResult<bool>> WithdrawProposalAsync(int proposalId, int userId);
        Task<ServiceResult<ReturnTenderProposalDTO>> ReviewProposalAsync(int proposalId, ProposalStatus newStatus);
        Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId);
        Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsForTenderAsync(int tenderId);

        Task<ServiceResult<bool>> CompleteTenderAsync(int tenderId);
        Task<ServiceResult<bool>> ValidateTenderProposalAsync(TenderProposal proposal);
        Task<ServiceResult<decimal>> CalculateTotalTenderValueAsync(int tenderId);
    }
}

