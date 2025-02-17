using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Helpers;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class MedicineUsageService(IUnitOfWork _unitOfWork, IMapper _mapper) : IMedicineUsageService
    {

        public async Task<ServiceResult<ReturnMedicineUsageDTO>> CreateUsageAsync(
            CreateMedicineUsageDTO createUsageDTO,
            int userId)
        {
            var result = new ServiceResult<ReturnMedicineUsageDTO>();


            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(createUsageDTO.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine not found for ID {createUsageDTO.MedicineId}");
            }

            var usage = _mapper.Map<MedicineUsage>(createUsageDTO);
            usage.UsedByUserId = userId;
            usage.UsageDate = DateTime.UtcNow;

            var createdUsage = await _unitOfWork.MedicineUsageRepository.CreateUsageAsync(usage);

            await _unitOfWork.CompleteAsync();
            result.Data = _mapper.Map<ReturnMedicineUsageDTO>(createdUsage);
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


        public async Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsagesByUserIdAsync(int userId)
        {
            var result = new ServiceResult<ReturnMedicineUsageDTO>();

            var usages = await _unitOfWork.MedicineUsageRepository.GetUsagesByUserIdAsync(userId);
            if (usages == null)
            {
                throw new KeyNotFoundException($"Usages for user with ID {userId} not found.");
            }

            result.Data = _mapper.Map<ReturnMedicineUsageDTO>(usages);
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

    }
}
