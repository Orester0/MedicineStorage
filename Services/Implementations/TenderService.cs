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
                tender.Status = TenderStatus.Created;
                tender.PublishDate = DateTime.UtcNow;
                tender.CreatedByUserId = userId;

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

        public async Task<ServiceResult<ReturnTenderItemDTO>> AddTenderItemAsync(CreateTenderItemDTO tenderItemDto)
        {
            var result = new ServiceResult<ReturnTenderItemDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderItemDto.TenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Created)
                {
                    result.Errors.Add("Items can only be added to draft tenders.");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(tenderItemDto.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found.");
                    return result;
                }

                var tenderItem = _mapper.Map<TenderItem>(tenderItemDto);
                var addedTenderItem = await _unitOfWork.TenderItemRepository.AddAsync(tenderItem);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderItemDTO>(addedTenderItem);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"An error occurred while adding tender item: {ex.Message}");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);

                if (tender.Status != TenderStatus.Created)
                {
                    result.Errors.Add("Only created tenders can be published.");
                    return result;
                }

                var tenderItems = await _unitOfWork.TenderItemRepository.GetByTenderIdAsync(tenderId);
                if (tenderItems == null || !tenderItems.Any())
                {
                    result.Errors.Add("Tender must have at least one item before publishing.");
                    return result;
                }

                tender.Status = TenderStatus.Published;
                tender.PublishDate = DateTime.UtcNow;
                tender.OpenedByUserId = userId;

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

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(CreateTenderProposalDTO proposalDto, List<CreateTenderProposalItemDTO> proposalItemsDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(proposalDto.TenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Published)
                {
                    result.Errors.Add("Proposals can only be submitted to published tenders.");
                    return result;
                }

                if (DateTime.UtcNow > tender.DeadlineDate)
                {
                    result.Errors.Add("Tender deadline has passed.");
                    return result;
                }

                // Validate proposal items match tender items
                var tenderItems = await _unitOfWork.TenderItemRepository.GetByTenderIdAsync(proposalDto.TenderId);
                foreach (var tenderItem in tenderItems)
                {
                    var matchingProposalItem = proposalItemsDto.FirstOrDefault(pi => pi.MedicineId == tenderItem.MedicineId);
                    if (matchingProposalItem == null)
                    {
                        result.Errors.Add($"Missing proposal item for medicine ID {tenderItem.MedicineId}.");
                        return result;
                    }

                    if (matchingProposalItem.Quantity > tenderItem.RequiredQuantity)
                    {
                        result.Errors.Add($"Proposed quantity for medicine ID {tenderItem.MedicineId} exceeds required quantity.");
                        return result;
                    }
                }

                var proposal = new TenderProposal
                {
                    TenderId = proposalDto.TenderId,
                    CreatedByUserId = userId,
                    TotalPrice = proposalDto.TotalPrice,
                    SubmissionDate = DateTime.UtcNow,
                    Status = ProposalStatus.Submitted
                };

                var addedProposal = await _unitOfWork.TenderProposalRepository.AddAsync(proposal);

                // Add Proposal Items
                var proposalItems = proposalItemsDto.Select(itemDto => new TenderProposalItem
                {
                    TenderProposalId = addedProposal.Id,
                    MedicineId = itemDto.MedicineId,
                    UnitPrice = itemDto.UnitPrice,
                    Quantity = itemDto.Quantity
                }).ToList();

                foreach (var item in proposalItems)
                {
                    await _unitOfWork.TenderProposalItemRepository.AddAsync(item);
                }
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderProposalDTO>(addedProposal);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"An error occurred while submitting the proposal: {ex.Message}");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId, int userId)
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
                tender.ClosedByUserId = userId;

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

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId, int userId)
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
                tender.WinnerSelectedByUserId = userId;
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

        // GET methods
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

        public async Task<ServiceResult<IEnumerable<ReturnTenderItemDTO>>> GetTenderItemsAsync(int tenderId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderItemDTO>>();

            try
            {
                var tenderItems = await _unitOfWork.TenderItemRepository.GetByTenderIdAsync(tenderId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderItemDTO>>(tenderItems);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"An error occurred while retrieving tender items: {ex.Message}");
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

        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>> GetTenderProposalItemsAsync(int proposalId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>();

            try
            {
                var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetByProposalIdAsync(proposalId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalItemDTO>>(proposalItems);
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"An error occurred while retrieving tender proposal items: {ex.Message}");
                return result;
            }
        }
    }
}