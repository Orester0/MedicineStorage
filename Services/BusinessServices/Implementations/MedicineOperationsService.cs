using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using CloudinaryDotNet.Actions;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class MedicineOperationsService(IUnitOfWork _unitOfWork, IMapper _mapper) : IMedicineOperationsService
    {

        public async Task<ServiceResult<PagedList<ReturnMedicineRequestDTO>>> GetAllRequestsAsync(MedicineRequestParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnMedicineRequestDTO>>();
            var (requests, totalCount) = await _unitOfWork.MedicineRequestRepository.GetAllAsync(parameters);
            var requestDtos = _mapper.Map<List<ReturnMedicineRequestDTO>>(requests);
            result.Data = new PagedList<ReturnMedicineRequestDTO>(
                requestDtos,
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );
            return result;
        }


        public async Task<ServiceResult<PagedList<ReturnMedicineUsageDTO>>> GetAllUsagesAsync(MedicineUsageParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnMedicineUsageDTO>>();
            var (usages, totalCount) = await _unitOfWork.MedicineUsageRepository.GetAllAsync(parameters);
            var usageDtos = _mapper.Map<List<ReturnMedicineUsageDTO>>(usages);
            result.Data = new PagedList<ReturnMedicineUsageDTO>(
                usageDtos,
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );
            return result;
        }

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(id);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request with ID {id} not found.");
                return result;
            }

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            return result;
        }
        public async Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsageByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnMedicineUsageDTO>();
            var usage = await _unitOfWork.MedicineUsageRepository.GetByIdAsync(id);
            if (usage == null)
            {
                throw new KeyNotFoundException($"Usage with ID {id} not found.");
            }

            result.Data = _mapper.Map<ReturnMedicineUsageDTO>(usage);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsRequestedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>();
            var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsRequestedByUserIdAsync(userId);
            result.Data = _mapper.Map<IEnumerable<ReturnMedicineRequestDTO>>(requests);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsApprovedByUserId(int userId)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>();
            var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsApprovedByUserIdAsync(userId);
            result.Data = _mapper.Map<IEnumerable<ReturnMedicineRequestDTO>>(requests);
            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>> GetRequestsForMedicineId(int medicineId)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineRequestDTO>>();
            var requests = await _unitOfWork.MedicineRequestRepository.GetRequestsForMedicineIdAsync(medicineId);
            result.Data = _mapper.Map<IEnumerable<ReturnMedicineRequestDTO>>(requests);
            return result;
        }
        public async Task<ServiceResult<ReturnMedicineRequestDTO>> GetRequestByUsageIdAsync(int usageId)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var request = await _unitOfWork.MedicineRequestRepository.GetRequestByUsageIdAsync(usageId);
            if (request == null)
            {

                throw new KeyNotFoundException($"Request not found for the given usage");
            }

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            return result;
        }
        public async Task<ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>> GetUsagesByRequestIdAsync(int requestId)
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineUsageDTO>>();
            var usages = await _unitOfWork.MedicineUsageRepository.GetUsagesByRequestIdAsync(requestId);
            result.Data = _mapper.Map<IEnumerable<ReturnMedicineUsageDTO>>(usages);
            return result;
        }




















        public async Task<ServiceResult<ReturnMedicineUsageDTO>> CreateUsageAsync(
            CreateMedicineUsageDTO createUsageDTO,
            int userId)
        {
            var result = new ServiceResult<ReturnMedicineUsageDTO>();

            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(createUsageDTO.MedicineRequestId);
            if (request == null)
            {

                throw new KeyNotFoundException($"Request not found for the given usage");
            }
            if (request.Status != RequestStatus.Approved)
            {

                throw new BadHttpRequestException("Request is not approved");
            }

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createUsageDTO.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine not found for ID {createUsageDTO.MedicineId}");
            }



            //if (createUsageDTO.Quantity > request.Quantity)
            //{
            //    result.Errors.Add("Invalid usage quantity");
            //    return result;
            //}

            var usage = _mapper.Map<MedicineUsage>(createUsageDTO);
            usage.UsedByUserId = userId;
            usage.UsageDate = DateTime.UtcNow;

            var createdUsage = await _unitOfWork.MedicineUsageRepository.CreateUsageAsync(usage);

            medicine.Stock -= createUsageDTO.Quantity;
            _unitOfWork.MedicineRepository.UpdateMedicine(medicine);

            await _unitOfWork.Complete();
            result.Data = _mapper.Map<ReturnMedicineUsageDTO>(createdUsage);
            return result;
        }



        public async Task<ServiceResult<ReturnMedicineRequestDTO>> CreateRequestAsync(
        CreateMedicineRequestDTO createRequestDTO,
        int userId)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createRequestDTO.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine not found for ID {createRequestDTO.MedicineId}");
            }

            var request = _mapper.Map<MedicineRequest>(createRequestDTO);
            request.RequestedByUserId = userId;
            request.RequestDate = DateTime.UtcNow;
            request.Status = RequestStatus.Pending;

            var requiresSpecialApproval = medicine.RequiresSpecialApproval;
            request.Status = requiresSpecialApproval
                ? RequestStatus.PedingWithSpecial
                : RequestStatus.Pending;

            var createdRequest = await _unitOfWork.MedicineRequestRepository.CreateRequestAsync(request);
            await _unitOfWork.Complete();

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(createdRequest);
            return result;
        }

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> ApproveRequestAsync(
            int requestId,
            int userId,
            bool isSpecialApproval = false)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request not found for ID {requestId}");
            }

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine not found for ID {request.MedicineId}");
            }

            if (medicine.RequiresSpecialApproval && !isSpecialApproval)
            {

                throw new BadHttpRequestException("Medicine in the request requires special approval");
            }

            if (request.Status != RequestStatus.Pending &&
                request.Status != RequestStatus.PedingWithSpecial)
            {

                throw new BadHttpRequestException("Request cannot be approved in its current state");
            }

            if (request.Quantity > medicine.Stock)
            {

                throw new BadHttpRequestException("Insufficient stock for approval");
            }

            request.Status = RequestStatus.Approved;
            request.ApprovedByUserId = userId;
            request.ApprovalDate = DateTime.UtcNow;

            medicine.Stock -= request.Quantity;
            _unitOfWork.MedicineRepository.UpdateMedicine(medicine);

            _unitOfWork.MedicineRequestRepository.UpdateRequest(request);
            await _unitOfWork.Complete();

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            return result;
        }


        public async Task<ServiceResult<ReturnMedicineRequestDTO>> RejectRequestAsync(
            int requestId,
            int userId,
            bool isSpecialApproval = false)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request not found for ID {requestId}");
            }

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine not found for ID {request.MedicineId}");
            }
            if (medicine.RequiresSpecialApproval && !isSpecialApproval)
            {
                throw new BadHttpRequestException("Medicine in the request requires special approval");
            }

            if (request.Status != RequestStatus.Pending &&
                request.Status != RequestStatus.PedingWithSpecial)
            {
                throw new BadHttpRequestException("Request cannot be rejected in its current state");
            }

            request.Status = RequestStatus.Rejected;
            request.ApprovedByUserId = userId;
            request.ApprovalDate = DateTime.UtcNow;

            _unitOfWork.MedicineRequestRepository.UpdateRequest(request);
            await _unitOfWork.Complete();

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteRequestAsync(int requestId)
        {
            var result = new ServiceResult<bool>();
            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request not found for ID {requestId}");
            }

            if (request.Status != RequestStatus.Pending)
            {

                throw new BadHttpRequestException("Only pending requests can be deleted");
            }

            await _unitOfWork.MedicineRequestRepository.DeleteRequestAsync(request.Id);
            await _unitOfWork.Complete();

            result.Data = true;
            return result;
        }

    }
}
