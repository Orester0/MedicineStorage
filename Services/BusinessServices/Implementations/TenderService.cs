using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.Tender;
using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class TenderService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITenderService
    {
        public async Task<ServiceResult<PagedList<Tender>>> GetAllTendersAsync(TenderParams tenderParams)
        {
            var result = new ServiceResult<PagedList<Tender>>();
            try
            {
                var (tenders, totalCount) = await _unitOfWork.TenderRepository.GetAllTendersAsync(tenderParams);
                result.Data = new PagedList<Tender>(tenders.ToList(), totalCount, tenderParams.PageNumber, tenderParams.PageSize);
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving tenders");
            }
            return result;
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
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving tender");
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersCreatedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();

            try
            {
                var tenders = await _unitOfWork.TenderRepository.GetTendersCreatedByUserIdAsync(userId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving tenders");
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersAwardedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();

            try
            {
                var tenders = await _unitOfWork.TenderRepository.GetTendersAwardedByUserIdAsync(userId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving tenders");
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderItemDTO>>> GetItemsByTenderId(int tenderId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderItemDTO>>();

            try
            {
                var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderItemDTO>>(tenderItems);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while retrieving tender items");
                return result;
            }
        }


        public async Task<ServiceResult<ReturnTenderProposalDTO>> GetProposalByIdAsync(int proposalId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetProposalByIdAsync(proposalId);
                result.Data = _mapper.Map<ReturnTenderProposalDTO>(proposal);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving proposal");
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsByTenderId(int tenderId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalDTO>>();

            try
            {
                var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tenderId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalDTO>>(proposals);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving proposals");
                return result;
            }
        }


        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsCreatedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalDTO>>();

            try
            {
                var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsCreatedByUserIdAsync(userId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalDTO>>(proposals);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Error retrieving proposal");
                return result;
            }
        }


        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>> GetItemsByProposalId(int proposalId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>();

            try
            {
                var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetItemsByProposalIdAsync(proposalId);
                result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalItemDTO>>(proposalItems);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while retrieving tender proposal items");
                return result;
            }
        }













        public async Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                if (tenderDto.DeadlineDate <= DateTime.UtcNow)
                {
                    result.Errors.Add("Deadline date must be in the future.");
                    return result;
                }

                var tender = _mapper.Map<Tender>(tenderDto);
                tender.Status = TenderStatus.Created;

                Console.WriteLine(userId);

                tender.CreatedByUserId = userId;

                var addedTender = await _unitOfWork.TenderRepository.CreateTenderAsync(tender);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderDTO>(addedTender);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Couldnt create tender");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderItemDTO>> AddTenderItemAsync(int tenderId, CreateTenderItemDTO tenderItemDto)
        {
            var result = new ServiceResult<ReturnTenderItemDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Created)
                {
                    result.Errors.Add("TenderItems can only be added to draft tenders.");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(tenderItemDto.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("ReturnMedicineDTO not found.");
                    return result;
                }


                var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);

                if (tenderItems != null)
                {
                    var existingItem = tenderItems.Any(x => x.MedicineId == tenderItemDto.MedicineId);

                    if (existingItem)
                    {
                        result.Errors.Add("This medicine is already in the tender.");
                        return result;
                    }
                }


                if (tenderItemDto.RequiredQuantity <= 0)
                {
                    result.Errors.Add("Required quantity must be greater than 0.");
                    return result;
                }

                var tenderItem = _mapper.Map<TenderItem>(tenderItemDto);
                tenderItem.TenderId = tenderId;
                var addedTenderItem = await _unitOfWork.TenderItemRepository.CreateTenderItemAsync(tenderItem);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderItemDTO>(addedTenderItem);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while adding tender item");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.CreatedByUserId != userId)
                {
                    result.Errors.Add("Cannot publish not yours tender");
                    return result;
                }


                if (tender.Status != TenderStatus.Created)
                {
                    result.Errors.Add("Only created tenders can be published.");
                    return result;
                }

                if (tender.DeadlineDate <= DateTime.UtcNow)
                {
                    result.Errors.Add("Cannot publish tender with passed deadline date.");
                    return result;
                }

                var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);
                if (!tenderItems.Any())
                {
                    result.Errors.Add("Tender must have at least one item before publishing.");
                    return result;
                }

                tender.Status = TenderStatus.Published;
                tender.PublishDate = DateTime.UtcNow;
                tender.OpenedByUserId = userId;

                _unitOfWork.TenderRepository.UpdateTender(tender);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderDTO>(tender);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Couldnt publish tender");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(int tenderId, CreateTenderProposalDTO proposalDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Published)
                {
                    result.Errors.Add("TenderProposals can only be submitted to published tenders.");
                    return result;
                }

                if (DateTime.UtcNow > tender.DeadlineDate)
                {
                    result.Errors.Add("Tender deadline has passed.");
                    return result;
                }


                var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsCreatedByUserIdAsync(userId);
                if (proposals != null)
                {
                    var existingProposal = proposals.Any(x => x.TenderId == tenderId);
                    if (existingProposal)
                    {
                        result.Errors.Add("You have already submitted a proposal for this tender.");
                        return result;
                    }
                }

                var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);
                decimal calculatedTotalPrice = 0;

                foreach (var tenderItem in tenderItems)
                {
                    var matchingProposalItem = proposalDto.ProposalItemsDTOs.FirstOrDefault(pi => pi.MedicineId == tenderItem.MedicineId);
                    if (matchingProposalItem == null)
                    {
                        result.Errors.Add($"Missing proposal item for medicine ID {tenderItem.MedicineId}.");
                        return result;
                    }

                    if (matchingProposalItem.Quantity != tenderItem.RequiredQuantity)
                    {
                        result.Errors.Add($"Not same quantity needed.");
                        return result;
                    }

                    if (matchingProposalItem.UnitPrice <= 0)
                    {
                        result.Errors.Add($"Invalid unit price for medicine ID {tenderItem.MedicineId}.");
                        return result;
                    }

                    calculatedTotalPrice += matchingProposalItem.Quantity * matchingProposalItem.UnitPrice;
                }

                if (Math.Abs(calculatedTotalPrice - proposalDto.TotalPrice) > 0.01m)
                {
                    result.Errors.Add("Total price does not match sum of proposal items.");
                    return result;
                }

                var proposal = new TenderProposal
                {
                    TenderId = tenderId,
                    CreatedByUserId = userId,
                    TotalPrice = proposalDto.TotalPrice,
                    SubmissionDate = DateTime.UtcNow,
                    Status = ProposalStatus.Submitted
                };

                var addedProposal = await _unitOfWork.TenderProposalRepository.CreateTenderProposalAsync(proposal);


                await _unitOfWork.Complete();


                foreach (var itemDto in proposalDto.ProposalItemsDTOs)
                {
                    var proposalItem = new TenderProposalItem
                    {
                        TenderProposalId = addedProposal.Id,
                        MedicineId = itemDto.MedicineId,
                        UnitPrice = itemDto.UnitPrice,
                        Quantity = itemDto.Quantity
                    };
                    await _unitOfWork.TenderProposalItemRepository.CreateTenderProposalItemAsync(proposalItem);
                }

                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderProposalDTO>(addedProposal);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while submitting the proposal");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            try
            {
                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Published)
                {
                    result.Errors.Add("Only published tenders can be closed.");
                    return result;
                }

                var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tenderId);
                if (!proposals.Any())
                {
                    result.Errors.Add("Cannot close tender with no proposals.");
                    return result;
                }

                tender.Status = TenderStatus.Closed;
                tender.ClosingDate = DateTime.UtcNow;
                tender.ClosedByUserId = userId;

                _unitOfWork.TenderRepository.UpdateTender(tender);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderDTO>(tender);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Couldnt close tender");
                return result;
            }
        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetProposalByIdAsync(proposalId);
                if (proposal == null)
                {
                    result.Errors.Add("Proposal not found.");
                    return result;
                }

                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(proposal.TenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Closed)
                {
                    result.Errors.Add("Tender must be closed before selecting a winning proposal.");
                    return result;
                }


                proposal.Status = ProposalStatus.Accepted;
                _unitOfWork.TenderProposalRepository.UpdateTenderProposal(proposal);

                var otherProposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tender.Id);
                foreach (var otherProposal in otherProposals.Where(p => p.Id != proposalId))
                {
                    otherProposal.Status = ProposalStatus.Rejected;
                    _unitOfWork.TenderProposalRepository.UpdateTenderProposal(otherProposal);
                }

                tender.Status = TenderStatus.Awarded;
                tender.WinnerSelectedByUserId = userId;

                _unitOfWork.TenderRepository.UpdateTender(tender);

                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnTenderProposalDTO>(proposal);
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"Couldnt award tender");
                return result;
            }
        }




        // GET methods


        public async Task<ServiceResult<bool>> ExecuteTenderItemAsync(int tenderItemId, int proposalId, int userId)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetProposalByIdAsync(proposalId);
                if (proposal == null || proposal.Status != ProposalStatus.Accepted)
                {
                    result.Errors.Add("Invalid proposal.");
                    return result;
                }

                var tenderItem = await _unitOfWork.TenderItemRepository.GetByIdAsync(tenderItemId);
                if (tenderItem == null)
                {
                    result.Errors.Add("Tender item not found.");
                    return result;
                }

                if (tenderItem.Status == TenderItemStatus.Executed)
                {
                    result.Errors.Add("Tender item has already been executed.");
                    return result;
                }

                var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderItem.TenderId);
                if (tender == null)
                {
                    result.Errors.Add("Tender not found.");
                    return result;
                }


                if (tender.Status != TenderStatus.Awarded && tender.Status != TenderStatus.Executing)
                {
                    result.Errors.Add("Tender must be in Awarded or Executing status.");
                    return result;
                }

                var medicine = tenderItem.Medicine;
                if (medicine == null)
                {
                    result.Errors.Add("No medicine found.");
                    return result;
                }

                var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetItemsByProposalIdAsync(proposalId);
                if (proposalItems == null)
                {
                    result.Errors.Add("Proposal items for the given tender item not found.");
                    return result;
                }

                var proposalItem = proposalItems.FirstOrDefault(a => a.MedicineId == medicine.Id);
                if (proposalItem == null)
                {
                    result.Errors.Add("Proposal item for medicine not found.");
                    return result;
                }

                var createdMedicineSupply = new MedicineSupply
                {
                    TenderProposalItemId = proposalItem.Id,
                    Quantity = proposalItem.Quantity,
                    TransactionDate = DateTime.UtcNow
                };
                await _unitOfWork.MedicineSupplyRepository.CreateMedicineSupplyAsync(createdMedicineSupply);

                tenderItem.Status = TenderItemStatus.Executed;
                _unitOfWork.TenderItemRepository.UpdateTenderItem(tenderItem);

                var updatedMedicine = tenderItem.Medicine;
                if (updatedMedicine != null)
                {
                    updatedMedicine.Stock += createdMedicineSupply.Quantity;
                    _unitOfWork.MedicineRepository.UpdateMedicine(updatedMedicine);
                }
                else
                {
                    result.Errors.Add($"ReturnMedicineDTO not found for tender item with ID {tenderItem.Id}");
                    return result;
                }

                var allTenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderItem.TenderId);
                if (allTenderItems.All(x => x.Status == TenderItemStatus.Executed))
                {
                    tender.Status = TenderStatus.Executed;
                }
                else
                {
                    tender.Status = TenderStatus.Executing;
                }

                _unitOfWork.TenderRepository.UpdateTender(tender);
                await _unitOfWork.Complete();

                result.Data = true;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while executing the tender item");
                return result;
            }
        }

        public async Task<ServiceResult<bool>> ExecuteTenderAsync(int proposalId, int userId)
        {
            var result = new ServiceResult<bool>();
            try
            {
                var proposal = await _unitOfWork.TenderProposalRepository.GetProposalByIdAsync(proposalId);
                if (proposal == null || proposal.Status != ProposalStatus.Accepted)
                {
                    result.Errors.Add("Proposal not found or is not the winning proposal.");
                    return result;
                }

                var tender = await _unitOfWork.TenderRepository.GetTenderByProposalIdAsync(proposalId);
                if (tender == null)
                {
                    result.Errors.Add("Tender for proposal not found.");
                    return result;
                }

                if (tender.Status != TenderStatus.Awarded)
                {
                    result.Errors.Add("Tender must be in Awarded status to be executed.");
                    return result;
                }

                var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tender.Id);
                if (!tenderItems.Any())
                {
                    result.Errors.Add("Tender items for tender not found.");
                    return result;
                }

                var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetItemsByProposalIdAsync(proposalId);
                if (!proposalItems.Any())
                {
                    result.Errors.Add("Proposal items for the given tender item not found.");
                    return result;
                }

                foreach (var tenderItem in tenderItems)
                {
                    var proposalItem = proposalItems.FirstOrDefault(x => x.MedicineId == tenderItem.MedicineId);
                    if (proposalItem != null)
                    {
                        var transaction = new MedicineSupply
                        {
                            TenderProposalItemId = proposalItem.Id,
                            Quantity = proposalItem.Quantity,
                            TransactionDate = DateTime.UtcNow
                        };

                        await _unitOfWork.MedicineSupplyRepository.CreateMedicineSupplyAsync(transaction);
                        tenderItem.Status = TenderItemStatus.Executed;
                        _unitOfWork.TenderItemRepository.UpdateTenderItem(tenderItem);


                        var updatedMedicine = tenderItem.Medicine;
                        if (updatedMedicine != null)
                        {
                            updatedMedicine.Stock += transaction.Quantity;
                            _unitOfWork.MedicineRepository.UpdateMedicine(updatedMedicine);
                        }
                        else
                        {
                            result.Errors.Add($"ReturnMedicineDTO not found for tender item with ID {tenderItem.Id}");
                            return result;
                        }
                    }
                    else
                    {
                        result.Errors.Add($"Proposal item not found for tender item with medicine ID {tenderItem.MedicineId}");
                        return result;
                    }
                }
                tender.Status = TenderStatus.Executed;
                _unitOfWork.TenderRepository.UpdateTender(tender);


                await _unitOfWork.Complete();

                result.Data = true;
                return result;
            }
            catch (Exception)
            {
                result.Errors.Add($"An error occurred while executing the tender");
                return result;
            }
        }



    }
}