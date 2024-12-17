using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class TenderService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITenderService
    {
        public async Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = _mapper.Map<Tender>(tenderDto);

                tender.Status = TenderStatus.Draft;
                tender.PublishDate = DateTime.UtcNow;
                tender.CreatedByUserId = userId; // Прив'язка до користувача

                if (tender.Items == null || !tender.Items.Any())
                {
                    result.Errors.Add("Tender must have at least one item.");
                    return result;
                }

                var addedTender = await _unitOfWork.TenderRepository.AddAsync(tender);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderDTO>(addedTender);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> UpdateTenderAsync(ReturnTenderDTO tenderDto)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var existingTender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderDto.Id);

                if (existingTender.Status != TenderStatus.Draft)
                {
                    result.Errors.Add("Only draft tenders can be updated.");
                    return result;
                }

                var tender = _mapper.Map<Tender>(tenderDto);
                await _unitOfWork.TenderRepository.UpdateAsync(tender);
                await _unitOfWork.Complete();

                result.Data = tenderDto;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<bool>> DeleteTenderAsync(int tenderId)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);

                if (tender.Status != TenderStatus.Draft)
                {
                    result.Errors.Add("Only draft tenders can be deleted.");
                    return result;
                }

                await _unitOfWork.TenderRepository.DeleteAsync(tenderId);
                await _unitOfWork.Complete();

                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);

                if (tender.Status != TenderStatus.Draft)
                {
                    result.Errors.Add("Only draft tenders can be published.");
                    return result;
                }

                tender.Status = TenderStatus.Published;
                tender.PublishDate = DateTime.UtcNow;

                await _unitOfWork.TenderRepository.UpdateAsync(tender);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderDTO>(tender);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);

                if (tender.Status != TenderStatus.Published)
                {
                    result.Errors.Add("Only published tenders can be closed.");
                    return result;
                }

                tender.Status = TenderStatus.Closed;
                tender.DeadlineDate = DateTime.UtcNow;

                await _unitOfWork.TenderRepository.UpdateAsync(tender);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderDTO>(tender);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> GetTenderByIdAsync(int tenderId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
                result.Data = _mapper.Map<ReturnTenderDTO>(tender);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetAllTendersAsync()
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();

            try
            {
                var tenders = await _unitOfWork.TenderRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersByStatusAsync(TenderStatus status)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();

            try
            {
                var tenders = await _unitOfWork.TenderRepository.GetTendersByStatusAsync(status);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersByUserAsync(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();

            try
            {
                var tenders = await _unitOfWork.TenderRepository.GetTendersByUserAsync(userId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }



        public async Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(CreateTenderProposalDTO proposalDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                // Map the DTO to the TenderProposal model
                var proposal = _mapper.Map<TenderProposal>(proposalDto);

                // Set the CreatedByUserId
                proposal.DistributorId = userId;

                // Validate the proposal
                var validationResult = await ValidateTenderProposalAsync(proposal);
                if (!validationResult.Success)
                {
                    // Transfer validation errors to the result and return
                    result.Errors.AddRange(validationResult.Errors);
                    return result;
                }

                // Set additional proposal properties
                proposal.SubmissionDate = DateTime.UtcNow;
                proposal.Status = ProposalStatus.Submitted;

                // Add the proposal to the database
                var addedProposal = await _unitOfWork.TenderProposalRepository.AddAsync(proposal);
                await _unitOfWork.Complete();

                // Map the result to the return DTO
                result.Data = _mapper.Map<ReturnTenderProposalDTO>(addedProposal);
                return result;
            }
            catch (Exception ex)
            {
                // Add exception message to the errors
                result.Errors.Add($"An error occurred while submitting the proposal: {ex.Message}");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> UpdateProposalAsync(ReturnTenderProposalDTO proposalDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var existingProposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalDto.Id);

                // Check if the proposal belongs to the user
                if (existingProposal.DistributorId != userId)
                {
                    result.Errors.Add("You can only update your own proposals.");
                    return result;
                }

                if (existingProposal.Status != ProposalStatus.Submitted)
                {
                    result.Errors.Add("Only submitted proposals can be updated.");
                    return result;
                }

                var proposal = _mapper.Map<TenderProposal>(proposalDto);
                await _unitOfWork.TenderProposalRepository.UpdateAsync(proposal);
                await _unitOfWork.Complete();

                result.Data = proposalDto;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<bool>> WithdrawProposalAsync(int proposalId, int userId)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);

                // Check if the proposal belongs to the user
                if (proposal.DistributorId != userId)
                {
                    result.Errors.Add("You can only withdraw your own proposals.");
                    return result;
                }

                if (proposal.Status != ProposalStatus.Submitted)
                {
                    result.Errors.Add("Only submitted proposals can be withdrawn.");
                    return result;
                }

                proposal.Status = ProposalStatus.Rejected;
                await _unitOfWork.TenderProposalRepository.UpdateAsync(proposal);
                await _unitOfWork.Complete();

                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> ReviewProposalAsync(int proposalId, ProposalStatus newStatus)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);

                if (proposal.Status == ProposalStatus.Accepted ||
                    proposal.Status == ProposalStatus.Rejected)
                {
                    result.Errors.Add("Proposal has already been finalized.");
                    return result;
                }

                proposal.Status = newStatus;
                await _unitOfWork.TenderProposalRepository.UpdateAsync(proposal);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderProposalDTO>(proposal);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(proposal.TenderId);

                if (tender.Status != TenderStatus.Closed)
                {
                    result.Errors.Add("Tender must be closed before selecting a winning proposal.");
                    return result;
                }

                proposal.Status = ProposalStatus.Accepted;
                await _unitOfWork.TenderProposalRepository.UpdateAsync(proposal);

                tender.Status = TenderStatus.Awarded;
                await _unitOfWork.TenderRepository.UpdateAsync(tender);

                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderProposalDTO>(proposal);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsForTenderAsync(int tenderId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalDTO>>();

            try
            {
                var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tenderId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalDTO>>(proposals);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<bool>> CompleteTenderAsync(int tenderId)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);

                if (tender.Status != TenderStatus.Awarded)
                {
                    result.Errors.Add("Only awarded tenders can be marked as complete.");
                    return result;
                }

                tender.Status = TenderStatus.Closed;
                await _unitOfWork.TenderRepository.UpdateAsync(tender);
                await _unitOfWork.Complete();

                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ServiceResult<bool>> ValidateTenderProposalAsync(TenderProposal proposal)
        {
            var result = new ServiceResult<bool>();

            try
            {
                if (proposal == null)
                {
                    result.Errors.Add("Proposal cannot be null.");
                    result.Data = false;
                    return result;
                }

                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(proposal.TenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    result.Data = false;
                    return result;
                }

                if (tender.Status != TenderStatus.Published)
                {
                    result.Errors.Add("Tender is not in published status.");
                    result.Data = false;
                    return result;
                }

                if (tender.DeadlineDate != null && DateTime.UtcNow > tender.DeadlineDate)
                {
                    result.Errors.Add("Tender deadline has passed.");
                    result.Data = false;
                    return result;
                }

                if (tender.Items == null || proposal.Items == null)
                {
                    result.Errors.Add("Tender or proposal items cannot be null.");
                    result.Data = false;
                    return result;
                }

                foreach (var tenderItem in tender.Items)
                {
                    var matchingProposalItem = proposal.Items.FirstOrDefault(pi => pi.MedicineId == tenderItem.MedicineId);

                    if (matchingProposalItem == null)
                    {
                        result.Errors.Add($"Missing proposal item for medicine ID {tenderItem.MedicineId}.");
                        result.Data = false;
                        return result;
                    }

                    if (matchingProposalItem.Quantity > tenderItem.RequiredQuantity)
                    {
                        result.Errors.Add($"Proposed quantity for medicine ID {tenderItem.MedicineId} exceeds required quantity.");
                        result.Data = false;
                        return result;
                    }
                }

                result.Data = true;
                return result;
            }
            catch (ArgumentNullException ex)
            {
                result.Errors.Add($"Null argument: {ex.Message}");
                result.Data = false;
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"An unexpected error occurred: {ex.Message}");
                result.Data = false;
                return result;
            }
        }

        public async Task<ServiceResult<decimal>> CalculateTotalTenderValueAsync(int tenderId)
        {
            var result = new ServiceResult<decimal>();

            try
            {
                var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tenderId);

                result.Data = proposals
                    .Where(p => p.Status == ProposalStatus.Accepted)
                    .Sum(p => p.TotalPrice);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"An error occurred while calculating the total tender value: {ex.Message}");
            }

            return result;
        }
    }
}