using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class MedicineOperationsService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<MedicineOperationsService> _logger) : IMedicineOperationsService
    {
        public async Task<ServiceResult<IEnumerable<MedicineRequestDTO>>> GetAllRequestsAsync()
        {
            var result = new ServiceResult<IEnumerable<MedicineRequestDTO>>();
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<MedicineRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all requests");
                result.Errors.Add("Failed to retrieve requests");
            }
            return result;
        }

        public async Task<ServiceResult<MedicineRequest>> CreateRequestAsync(CreateMedicineRequestDTO createRequestDTO, int userId)
        {
            var result = new ServiceResult<MedicineRequest>();
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

                // Determine initial status based on medicine's approval requirements
                if (medicine.RequiresSpecialApproval)
                {
                    request.Status = RequestStatus.ApprovalRequired;
                }
                else
                {
                    // Auto-process requests for medicines not requiring special approval
                    if (createRequestDTO.Quantity <= medicine.Stock)
                    {
                        request.Status = RequestStatus.Completed;

                        // Create usage record automatically
                        var usage = new MedicineUsage
                        {
                            MedicineId = request.MedicineId,
                            UsedByUserId = request.RequestedByUserId,
                            Quantity = request.Quantity,
                            UsageDate = DateTime.UtcNow,
                            Notes = $"Auto-generated from request {request.Id}"
                        };

                        await _unitOfWork.MedicineUsageRepository.AddUsageAsync(usage);
                        medicine.Stock -= request.Quantity;
                        _unitOfWork.MedicineRepository.Update(medicine);
                    }
                    else
                    {
                        request.Status = RequestStatus.Pending;
                    }
                }

                var createdRequest = await _unitOfWork.MedicineRequestRepository.AddRequestAsync(request);
                await _unitOfWork.Complete();

                result.Data = createdRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medicine request");
                result.Errors.Add("Failed to create request");
            }
            return result;
        }

        public async Task<ServiceResult<MedicineUsage>> CreateUsageAsync(CreateMedicineUsageDTO createUsageDTO, int userId)
        {
            var result = new ServiceResult<MedicineUsage>();
            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createUsageDTO.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
                    return result;
                }

                if (createUsageDTO.Quantity > medicine.Stock)
                {
                    result.Errors.Add("Insufficient stock");
                    return result;
                }

                var usage = _mapper.Map<MedicineUsage>(createUsageDTO);
                usage.UsedByUserId = userId;
                var createdUsage = await _unitOfWork.MedicineUsageRepository.AddUsageAsync(usage);

                medicine.Stock -= createUsageDTO.Quantity;
                _unitOfWork.MedicineRepository.Update(medicine);

                await _unitOfWork.Complete();
                result.Data = createdUsage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medicine usage");
                result.Errors.Add("Failed to create usage");
            }
            return result;
        }

        public async Task<ServiceResult<MedicineRequestDTO>> ApproveRequestAsync(int requestId, int userId)
        {
            var result = new ServiceResult<MedicineRequestDTO>();
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
                    result.Errors.Add("Request cannot be approved in its current state");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
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

                var usage = new MedicineUsage
                {
                    MedicineId = request.MedicineId,
                    UsedByUserId = request.RequestedByUserId,
                    Quantity = request.Quantity,
                    UsageDate = DateTime.UtcNow,
                    Notes = $"Generated from approved request {request.Id}"
                };

                await _unitOfWork.MedicineUsageRepository.AddUsageAsync(usage);
                medicine.Stock -= request.Quantity;

                await _unitOfWork.MedicineRequestRepository.UpdateRequestAsync(request);
                _unitOfWork.MedicineRepository.Update(medicine);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<MedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving medicine request");
                result.Errors.Add("Failed to approve request");
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<MedicineUsageDTO>>> GetAllUsagesAsync()
        {
            var result = new ServiceResult<IEnumerable<MedicineUsageDTO>>();
            try
            {
                var usages = await _unitOfWork.MedicineUsageRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<MedicineUsageDTO>>(usages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all usages");
                result.Errors.Add("Failed to retrieve usages");
            }
            return result;
        }


        public async Task<ServiceResult<MedicineRequestDTO>> ProcessSpecialApprovalRequestAsync(int requestId, bool isApproved, int userId)
        {
            var result = new ServiceResult<MedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                if (request.Status != RequestStatus.ApprovalRequired)
                {
                    result.Errors.Add("Request is not in approval required state");
                    return result;
                }

                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
                if (medicine == null)
                {
                    result.Errors.Add("Medicine not found");
                    return result;
                }

                if (isApproved)
                {
                    if (request.Quantity > medicine.Stock)
                    {
                        result.Errors.Add("Insufficient stock");
                        return result;
                    }

                    request.Status = RequestStatus.Completed;
                    request.ApprovedByUserId = userId;
                    request.ApprovalDate = DateTime.UtcNow;

                    var usage = new MedicineUsage
                    {
                        MedicineId = request.MedicineId,
                        UsedByUserId = request.RequestedByUserId,
                        Quantity = request.Quantity,
                        UsageDate = DateTime.UtcNow,
                        Notes = $"Approved from special request {request.Id}"
                    };

                    await _unitOfWork.MedicineUsageRepository.AddUsageAsync(usage);
                    medicine.Stock -= request.Quantity;
                    _unitOfWork.MedicineRepository.Update(medicine);
                }
                else
                {
                    request.Status = RequestStatus.Rejected;
                    request.ApprovedByUserId = userId;
                    request.ApprovalDate = DateTime.UtcNow;
                }

                await _unitOfWork.MedicineRequestRepository.UpdateRequestAsync(request);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<MedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing special approval request");
                result.Errors.Add("Failed to process special approval request");
            }
            return result;
        }

        public async Task<ServiceResult<MedicineRequestDTO>> GetRequestByIdAsync(int id)
        {
            var result = new ServiceResult<MedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(id);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                result.Data = _mapper.Map<MedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving request with id {id}");
                result.Errors.Add("Failed to retrieve request");
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<MedicineRequestDTO>>> GetRequestsByUserAsync(int userId)
        {
            var result = new ServiceResult<IEnumerable<MedicineRequestDTO>>();
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsByUserAsync(userId);
                result.Data = _mapper.Map<IEnumerable<MedicineRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving requests for user {userId}");
                result.Errors.Add("Failed to retrieve user requests");
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<MedicineRequestDTO>>> GetRequestsByStatusAsync(RequestStatus status)
        {
            var result = new ServiceResult<IEnumerable<MedicineRequestDTO>>();
            try
            {
                var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsByStatusAsync(status);
                result.Data = _mapper.Map<IEnumerable<MedicineRequestDTO>>(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving requests with status {status}");
                result.Errors.Add("Failed to retrieve requests by status");
            }
            return result;
        }

        public async Task<ServiceResult<MedicineRequestDTO>> UpdateRequestStatusAsync(int requestId, RequestStatus newStatus, int userId)
        {
            var result = new ServiceResult<MedicineRequestDTO>();
            try
            {
                var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    result.Errors.Add("Request not found");
                    return result;
                }

                request.Status = newStatus;
                request.ApprovedByUserId = userId;
                request.ApprovalDate = DateTime.UtcNow;

                await _unitOfWork.MedicineRequestRepository.UpdateRequestAsync(request);
                await _unitOfWork.Complete();

                result.Data = _mapper.Map<MedicineRequestDTO>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating request status for request {requestId}");
                result.Errors.Add("Failed to update request status");
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

                await _unitOfWork.MedicineRequestRepository.DeleteRequestAsync(request.Id);
                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting request {requestId}");
                result.Errors.Add("Failed to delete request");
            }
            return result;
        }

        public async Task<ServiceResult<MedicineUsageDTO>> GetUsageByIdAsync(int id)
        {
            var result = new ServiceResult<MedicineUsageDTO>();
            try
            {
                var usage = await _unitOfWork.MedicineUsageRepository.GetByIdAsync(id);
                if (usage == null)
                {
                    result.Errors.Add("Usage record not found");
                    return result;
                }

                result.Data = _mapper.Map<MedicineUsageDTO>(usage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving usage with id {id}");
                result.Errors.Add("Failed to retrieve usage record");
            }
            return result;
        }
    }
}
