using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using CloudinaryDotNet.Actions;
using MedicineStorage.Services.ApplicationServices.Implementations;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Patterns;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Models.AuditModels;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class MedicineRequestService(IUnitOfWork _unitOfWork, 
                                        IMapper _mapper, 
                                        INotificationTextFactory _notificationTextFactory, 
                                        INotificationService _notificationService) : IMedicineRequestService
    {
        public async Task<ServiceResult<PagedList<ReturnMedicineRequestDTO>>> GetPaginatedAudits(MedicineRequestParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnMedicineRequestDTO>>();
            var (requests, totalCount) = await _unitOfWork.MedicineRequestRepository.GetByParams(parameters);
            var requestDtos = _mapper.Map<List<ReturnMedicineRequestDTO>>(requests);
            result.Data = new PagedList<ReturnMedicineRequestDTO>(
                requestDtos,
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
            }
            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
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


        // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // //  // // 


        public async Task<ServiceResult<ReturnMedicineRequestDTO>> CreateRequestAsync(
        CreateMedicineRequestDTO createRequestDTO,
        int userId)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createRequestDTO.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {createRequestDTO.MedicineId} not found");
            }

            var request = _mapper.Map<MedicineRequest>(createRequestDTO);
            request.RequestedByUserId = userId;
            request.RequestDate = DateTime.UtcNow;
            request.Status = RequestStatus.Pending;

            var requiresSpecialApproval = medicine.RequiresSpecialApproval;
            request.Status = requiresSpecialApproval
                ? RequestStatus.PedingWithSpecial
                : RequestStatus.Pending;

            var createdRequest = await _unitOfWork.MedicineRequestRepository.AddAsync(request);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(createdRequest);
            return result;
        }

        public async Task<ServiceResult<ReturnMedicineRequestDTO>> ApproveRequestAsync(
            int requestId,
            int userId,
            List<string> userRoles)
        {
            var result = new ServiceResult<ReturnMedicineRequestDTO>();
            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found");
            }

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(request.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine not found for ID {request.MedicineId}");
            }

            if (medicine.RequiresSpecialApproval && !userRoles.Contains("Admin"))
            {
                throw new BadHttpRequestException("Medicine in the request requires admin approval");
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
            _unitOfWork.MedicineRepository.Update(medicine);
            _unitOfWork.MedicineRequestRepository.Update(request);



            var (title, message) = _notificationTextFactory.GetNotificationText(NotificationType.MedicineRequestApproved, medicine.Name);

            var notification = new Notification
            {
                UserId = request.RequestedByUserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationService.SendNotificationAsync(notification);


            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            return result;
        }


        public async Task<ServiceResult<ReturnMedicineRequestDTO>> RejectRequestAsync(
            int requestId,
            int userId,
            List<string> userRoles)
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

            if (medicine.RequiresSpecialApproval && !userRoles.Contains("Admin"))
            {
                throw new BadHttpRequestException("Medicine in the request requires admin approval");
            }

            if (request.Status != RequestStatus.Pending &&
                request.Status != RequestStatus.PedingWithSpecial)
            {
                throw new BadHttpRequestException("Request cannot be rejected in its current state");
            }

            request.Status = RequestStatus.Rejected;
            request.ApprovedByUserId = userId;
            request.ApprovalDate = DateTime.UtcNow;
            _unitOfWork.MedicineRequestRepository.Update(request);




            var (title, message) = _notificationTextFactory.GetNotificationText(NotificationType.MedicineRequestRejected, medicine.Name);
            var notification = new Notification
            {
                UserId = request.RequestedByUserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationService.SendNotificationAsync(notification);

            await _unitOfWork.CompleteAsync();
            result.Data = _mapper.Map<ReturnMedicineRequestDTO>(request);
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteRequestAsync(int requestId, int userId, List<string> userRoles)
        {
            var result = new ServiceResult<bool>();
            var request = await _unitOfWork.MedicineRequestRepository.GetByIdAsync(requestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request not found for ID {requestId}");
            }

            if (request.RequestedByUserId != userId && !userRoles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (request.Status != RequestStatus.Pending && request.Status != RequestStatus.PedingWithSpecial)
            {

                throw new BadHttpRequestException("Only pending requests can be deleted");
            }

            await _unitOfWork.MedicineRequestRepository.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync();

            result.Data = true;
            return result;
        }



    }
}
