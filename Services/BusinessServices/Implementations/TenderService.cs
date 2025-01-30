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
using System.Collections.Generic;
using MimeKit;
using Humanizer;
using System.CodeDom.Compiler;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class TenderService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITenderService
    {
        public async Task<ServiceResult<PagedList<ReturnTenderDTO>>> GetAllTendersAsync(TenderParams tenderParams)
        {
            var result = new ServiceResult<PagedList<ReturnTenderDTO>>();
            var (tenders, totalCount) = await _unitOfWork.TenderRepository.GetAllTendersAsync(tenderParams);
            result.Data = new PagedList<ReturnTenderDTO>(_mapper.Map<List<ReturnTenderDTO>>(tenders), totalCount, tenderParams.PageNumber, tenderParams.PageSize);
            return result;
        }

        public async Task<ServiceResult<ReturnTenderDTO>> GetTenderByIdAsync(int tenderId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();
            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
            result.Data = _mapper.Map<ReturnTenderDTO>(tender);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersCreatedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();
            var tenders = await _unitOfWork.TenderRepository.GetTendersCreatedByUserIdAsync(userId);
            result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderDTO>>> GetTendersAwardedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderDTO>>();
            var tenders = await _unitOfWork.TenderRepository.GetTendersAwardedByUserIdAsync(userId);
            result.Data = _mapper.Map<IEnumerable<ReturnTenderDTO>>(tenders);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderItemDTO>>> GetItemsByTenderId(int tenderId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderItemDTO>>();
            var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);
            result.Data = _mapper.Map<IEnumerable<ReturnTenderItemDTO>>(tenderItems);
            return result;
        }


        public async Task<ServiceResult<ReturnTenderProposalDTO>> GetProposalByIdAsync(int proposalId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();
            var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);
            result.Data = _mapper.Map<ReturnTenderProposalDTO>(proposal);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsByTenderId(int tenderId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalDTO>>();
            var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tenderId);
            result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalDTO>>(proposals);
            return result;
        }


        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalDTO>>> GetProposalsCreatedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalDTO>>();
            var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsCreatedByUserIdAsync(userId);
            result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalDTO>>(proposals);
            return result;
        }


        public async Task<ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>> GetItemsByProposalId(int proposalId)
        {
            var result = new ServiceResult<IEnumerable<ReturnTenderProposalItemDTO>>();



            var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetItemsByProposalIdAsync(proposalId);

            result.Data = _mapper.Map<IEnumerable<ReturnTenderProposalItemDTO>>(proposalItems);
            return result;
        }












        public async Task<ServiceResult<ReturnTenderDTO>> CreateTenderAsync(CreateTenderDTO tenderDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();
            var tender = _mapper.Map<Tender>(tenderDto);

            tender.Status = TenderStatus.Created;
            tender.CreatedByUserId = userId;

            var addedTender = await _unitOfWork.TenderRepository.CreateTenderAsync(tender);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnTenderDTO>(addedTender);
            return result;
        }

        public async Task<ServiceResult<ReturnTenderItemDTO>> AddTenderItemAsync(int tenderId, CreateTenderItemDTO tenderItemDto)
        {
            var result = new ServiceResult<ReturnTenderItemDTO>();


            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender with ID {tenderId} not found.");
            }

            if (tender.Status != TenderStatus.Created)
            {
                throw new BadHttpRequestException("TenderItems can only be added to draft tenders.");
            }

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(tenderItemDto.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {tenderItemDto.MedicineId} not found.");
            }


            var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);

            if (tenderItems != null)
            {
                var existingItem = tenderItems.Any(x => x.MedicineId == tenderItemDto.MedicineId);

                if (existingItem)
                {
                    throw new BadHttpRequestException($"Medicine is already in the tender.");
                }
            }


            var tenderItem = _mapper.Map<TenderItem>(tenderItemDto);
            tenderItem.TenderId = tenderId;
            var addedTenderItem = await _unitOfWork.TenderItemRepository.CreateTenderItemAsync(tenderItem);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnTenderItemDTO>(addedTenderItem);
            return result;
        }

        public async Task<ServiceResult<ReturnTenderDTO>> PublishTenderAsync(int tenderId, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();
            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender with ID {tenderId} not found.");
            }

            if (tender.CreatedByUserId != userId)
            {
                throw new BadHttpRequestException($"Cannot publish not yours tender.");
            }

            if (tender.Status != TenderStatus.Created)
            {
                throw new BadHttpRequestException($"Only draft tenders can be published.");
            }

            if (tender.DeadlineDate <= DateTime.UtcNow)
            {
                throw new BadHttpRequestException($"Cannot publish tender with passed deadline date.");
            }

            var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);
            if (!tenderItems.Any())
            {
                throw new BadHttpRequestException($"Tender must have at least one item before publishing.");
            }

            tender.Status = TenderStatus.Published;
            tender.PublishDate = DateTime.UtcNow;
            tender.OpenedByUserId = userId;

            _unitOfWork.TenderRepository.UpdateTender(tender);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnTenderDTO>(tender);
            return result;
        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SubmitProposalAsync(int tenderId, CreateTenderProposalDTO proposalDto, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();

            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender with ID {tenderId} not found.");
            }

            if (tender.Status != TenderStatus.Published)
            {
                throw new BadHttpRequestException($"TenderProposals can only be submitted to published tenders.");
            }

            if (DateTime.UtcNow > tender.DeadlineDate)
            {

                throw new BadHttpRequestException($"Tender deadline has passed.");
            }


            var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsCreatedByUserIdAsync(userId);
            if (proposals != null)
            {
                var existingProposal = proposals.Any(x => x.TenderId == tenderId);
                if (existingProposal)
                {
                    throw new BadHttpRequestException($"You have already submitted a proposal for this tender.");
                }
            }

            var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tenderId);
            decimal calculatedTotalPrice = 0;

            foreach (var tenderItem in tenderItems)
            {
                var matchingProposalItem = proposalDto.ProposalItemsDTOs.FirstOrDefault(pi => pi.MedicineId == tenderItem.MedicineId);
                if (matchingProposalItem == null)
                {

                    throw new KeyNotFoundException($"Missing proposal item for medicine ID {tenderItem.MedicineId}.");
                }

                if (matchingProposalItem.Quantity != tenderItem.RequiredQuantity)
                {
                    throw new BadHttpRequestException($"Not same quantity needed.");
                }

                if (matchingProposalItem.UnitPrice <= 0)
                {

                    throw new BadHttpRequestException($"Invalid unit price for medicine ID {tenderItem.MedicineId}.");
                }

                calculatedTotalPrice += matchingProposalItem.Quantity * matchingProposalItem.UnitPrice;
            }

            if (Math.Abs(calculatedTotalPrice - proposalDto.TotalPrice) > 0.01m)
            {
                throw new BadHttpRequestException("Total price does not match sum of proposal items.");
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


            await _unitOfWork.CompleteAsync();


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

            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnTenderProposalDTO>(addedProposal);
            return result;
        }

        public async Task<ServiceResult<ReturnTenderDTO>> CloseTenderAsync(int tenderId, int userId)
        {
            var result = new ServiceResult<ReturnTenderDTO>();

            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender with ID {tenderId} not found.");
            }

            if (tender.Status != TenderStatus.Published)
            {
                throw new BadHttpRequestException("Only published tenders can be closed.");
            }

            var proposals = await _unitOfWork.TenderProposalRepository.GetProposalsByTenderAsync(tenderId);
            if (!proposals.Any())
            {
                throw new BadHttpRequestException("Cannot close tender with no proposals.");
            }

            tender.Status = TenderStatus.Closed;
            tender.ClosingDate = DateTime.UtcNow;
            tender.ClosedByUserId = userId;

            _unitOfWork.TenderRepository.UpdateTender(tender);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnTenderDTO>(tender);
            return result;

        }

        public async Task<ServiceResult<ReturnTenderProposalDTO>> SelectWinningProposalAsync(int proposalId, int userId)
        {
            var result = new ServiceResult<ReturnTenderProposalDTO>();
            var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);
            if (proposal == null)
            {

                throw new KeyNotFoundException($"Proposal with ID {proposalId} not found.");
            }

            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(proposal.TenderId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender matching this proposal not found.");
            }

            if (tender.Status != TenderStatus.Closed)
            {

                throw new BadHttpRequestException("Tender must be closed before selecting a winning proposal.");
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

            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnTenderProposalDTO>(proposal);
            return result;
        }






        public async Task<ServiceResult<bool>> ExecuteTenderItemAsync(int tenderItemId, int proposalId, int userId)
        {
            var result = new ServiceResult<bool>();
            var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);
            if (proposal == null || proposal.Status != ProposalStatus.Accepted)
            {
                throw new KeyNotFoundException($"Proposal with ID {proposalId} not found.");
            }

            if (proposal.Status != ProposalStatus.Accepted)
            {

                throw new BadHttpRequestException($"Cannot execute not accepted proposal.");
            }

            var tenderItem = await _unitOfWork.TenderItemRepository.GetByIdAsync(tenderItemId);
            if (tenderItem == null)
            {

                throw new KeyNotFoundException($"Tender item with ID {proposalId} not found.");
            }

            if (tenderItem.Status == TenderItemStatus.Executed)
            {
                throw new BadHttpRequestException($"Tender item has already been executed.");
            }

            var tender = await _unitOfWork.TenderRepository.GetByIdAsync(tenderItem.TenderId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender with ID {proposalId} not found.");
            }


            if (tender.Status != TenderStatus.Awarded && tender.Status != TenderStatus.Executing)
            {
                throw new BadHttpRequestException($"Tender must be in Awarded or Executing status.");
            }

            var medicine = tenderItem.Medicine;
            if (medicine == null)
            {

                throw new KeyNotFoundException($"Medicine for tender item not found.");
            }

            var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetItemsByProposalIdAsync(proposalId);
            if (proposalItems == null)
            {
                throw new KeyNotFoundException($"Proposal items for the given tender item not found.");
            }

            var proposalItem = proposalItems.FirstOrDefault(a => a.MedicineId == medicine.Id);
            if (proposalItem == null)
            {

                throw new KeyNotFoundException($"Proposal item for medicine not found.");
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
                throw new KeyNotFoundException($"Medicine not found for tender item with ID {tenderItem.Id}");
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
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ExecuteTenderAsync(int proposalId, int userId)
        {
            var result = new ServiceResult<bool>();
            var proposal = await _unitOfWork.TenderProposalRepository.GetByIdAsync(proposalId);
            if (proposal == null || proposal.Status != ProposalStatus.Accepted)
            {
                throw new KeyNotFoundException($"Proposal with ID {proposalId} not found.");
            }
            if (proposal.Status != ProposalStatus.Accepted)
            {
                throw new BadHttpRequestException($"Cannot execute not accepted proposal.");
            }

            var tender = await _unitOfWork.TenderRepository.GetTenderByProposalIdAsync(proposalId);
            if (tender == null)
            {
                throw new KeyNotFoundException($"Tender for proposal not found.");
            }

            if (tender.Status != TenderStatus.Awarded)
            {
                throw new BadHttpRequestException($"Tender must be in Awarded status to be executed.");
            }

            var tenderItems = await _unitOfWork.TenderItemRepository.GetItemsByTenderIdAsync(tender.Id);
            if (!tenderItems.Any())
            {

                throw new KeyNotFoundException($"Tender items for tender not found.");
            }

            var proposalItems = await _unitOfWork.TenderProposalItemRepository.GetItemsByProposalIdAsync(proposalId);
            if (!proposalItems.Any())
            {
                throw new KeyNotFoundException($"Proposal items for the given tender item not found.");
            }

            foreach (var tenderItem in tenderItems)
            {
                var proposalItem = proposalItems.FirstOrDefault(x => x.MedicineId == tenderItem.MedicineId);
                if (proposalItem != null)
                {
                    var transaction = new MedicineSupply
                    {
                        MedicineId = proposalItem.MedicineId,
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

                        throw new KeyNotFoundException($"Medicine not found for tender item with ID {tenderItem.Id}");
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"Proposal item not found for tender item with medicine ID {tenderItem.MedicineId}");
                }
            }
            tender.Status = TenderStatus.Executed;
            _unitOfWork.TenderRepository.UpdateTender(tender);


            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }



    }
}