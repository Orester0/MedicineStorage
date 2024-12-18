﻿using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class MedicineOperationsService(IUnitOfWork _unitOfWork, IMapper _mapper) : IMedicineOperationsService
    {

        // USAGES
        public async Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsageByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnMedicineUsageDTO>();
            try
            {
                var usage = await _unitOfWork.MedicineUsageRepository.GetByIdAsync(id);
                if (usage == null)
                {
                    result.Errors.Add("Usage record not found");
                    return result;
                }

                result.Data = _mapper.Map<ReturnMedicineUsageDTO>(usage);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve usage record");
            }
            return result;
        }
        public async Task<ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>> GetAllUsagesAsync()
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>();
            try
            {
                var usages = await _unitOfWork.MedicineUsageRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineUsageDTO>>(usages);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve usages");
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>> GetUsagesByRequestIdAsync(int requestId)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>();
            try
            {
                var usages = await _unitOfWork.MedicineUsageRepository.GetUsagesByRequestIdAsync(requestId);
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineUsageDTO>>(usages);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve usages for request");
            }
            return result;
        }


        public async Task<ServiceResult<ReturnMedicineUsageDTO>> CreateUsageAsync(
            CreateMedicineUsageDTO createUsageDTO,
            int userId)
        {
            var result = new ServiceResult<ReturnMedicineUsageDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(createUsageDTO.MedicineRequestId);
                if (request == null || request.Status != RequestStatus.Approved)
                {
                    result.Errors.Add("Request not found or not approved");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createUsageDTO.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
                    return result;
                }



                if (createUsageDTO.Quantity > request.Quantity ||
                    createUsageDTO.Quantity > medicine.Stock)
                {
                    result.Errors.Add("Invalid usage quantity");
                    return result;
                }

                var usage = _mapper.Map<MedicineUsage>(createUsageDTO);
                usage.UsedByUserId = userId;
                usage.UsageDate = DateTime.UtcNow;

                var createdUsage = await _unitOfWork.MedicineUsageRepository.AddUsageAsync(usage);

                medicine.Stock -= createUsageDTO.Quantity;
                _unitOfWork.MedicineRepository.Update(medicine);

                await _unitOfWork.Complete();
                result.Data = _mapper.Map<ReturnMedicineUsageDTO>(createdUsage);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to create usage");
            }
            return result;
        }


        // REQUESTS

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(id);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve request");
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsByUserAsync(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>();
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsByUserAsync(userId);
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve user requests");
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsByStatusAsync(RequestStatus status)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>();
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsByStatusAsync(status);
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve requests by status");
            }
            return result;
        }


        public async Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetAllRequestsAsync()
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>();
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve requests");
            }
            return result;
        }

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByUsageIdAsync(int usageId)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetRequestByUsageIdAsync(usageId);
                if (request == null)
                {
                    result.Errors.Add("Request not found for the given usage");
                    return result;
                }

                result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            }
            catch (Exception ex)
            { 
                result.Errors.Add("Failed to retrieve request by usage");
            }
            return result;
        }

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> CreateRequestAsync(
        CreateMedicineRequestDTO createRequestDTO,
        int userId)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createRequestDTO.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
                    return result;
                }

                var request = _mapper.Map<MedicineRequest>(createRequestDTO);
                request.RequestedByUserId = userId;
                request.RequestDate = DateTime.UtcNow;
                request.Status = RequestStatus.Pending;

                var requiresSpecialApproval = medicine.RequiresSpecialApproval;
                request.Status = requiresSpecialApproval
                    ? RequestStatus.PedingWithSpecial
                    : RequestStatus.Pending;

                var createdRequest = await _unitOfWork.MedicineRequestRepository.AddRequestAsync(request);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnMedicineRequestDTO>(createdRequest);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to create request");
            }
            return result;
        }

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> ApproveRequestAsync(
            int requestId,
            int userId,
            bool isSpecialApproval = false)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
                    return result;
                }

                if (medicine.RequiresSpecialApproval && !isSpecialApproval)
                {
                    result.Errors.Add("This medicine requires special approval");
                    return result;
                }

                if (request.Status != RequestStatus.Pending &&
                    request.Status != RequestStatus.PedingWithSpecial)
                {
                    result.Errors.Add("Request cannot be approved in its current state");
                    return result;
                }

                if (request.Quantity > medicine.Stock)
                {
                    result.Errors.Add("Insufficient stock for approval");
                    return result;
                }

                request.Status = RequestStatus.Approved;
                request.ApprovedByUserId = userId;
                request.ApprovalDate = DateTime.UtcNow;

                medicine.Stock -= request.Quantity;
                _unitOfWork.MedicineRepository.Update(medicine);

                await _unitOfWork.MedicineRequestRepository.UpdateRequestAsync(request);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to approve request");
            }
            return result;
        }


        public async Task<ServiceResult<ReturnMedicineRequestDTO>> RejectRequestAsync(
            int requestId,
            int userId,
            bool isSpecialApproval = false)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
                    return result;
                }
                if (medicine.RequiresSpecialApproval && !isSpecialApproval)
                {
                    result.Errors.Add("This medicine requires special approval");
                    return result;
                }

                if (request.Status != RequestStatus.Pending &&
                    request.Status != RequestStatus.PedingWithSpecial)
                {
                    result.Errors.Add("Request cannot be rejected in its current state");
                    return result;
                }

                request.Status = RequestStatus.Rejected;
                request.ApprovedByUserId = userId;
                request.ApprovalDate = DateTime.UtcNow;

                await _unitOfWork.MedicineRequestRepository.UpdateRequestAsync(request);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to reject request");
            }
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteRequestAsync(int requestId)
        {
            var result = new ServiceResult<bool>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                if (request.Status != RequestStatus.Pending)
                {
                    result.Errors.Add("Only pending requests can be deleted");
                    return result;
                }

                await _unitOfWork.MedicineRequestRepository.DeleteRequestAsync(request.Id);
                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to delete request");
            }
            return result;
        }

    }
}
