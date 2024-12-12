using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.DTOs;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class MedicineService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        ILogger<MedicineService> _logger) : IMedicineService
    {
        public async Task<ServiceResult<IEnumerable<MedicineDTO>>> GetAllMedicinesAsync()
        {
            var result = new ServiceResult<IEnumerable<MedicineDTO>>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<MedicineDTO>>(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all medicines");
                result.Errors.Add("Failed to retrieve medicines");
            }

            return result;
        }

        public async Task<ServiceResult<MedicineDTO>> GetMedicineByIdAsync(int id)
        {
            var result = new ServiceResult<MedicineDTO>();

            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (medicine == null)
                {
                    result.Errors.Add($"Medicine with ID {id} not found");
                    return result;
                }

                result.Data = _mapper.Map<MedicineDTO>(medicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medicine {MedicineId}", id);
                result.Errors.Add("Failed to retrieve medicine");
            }

            return result;
        }

        public async Task<ServiceResult<MedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO)
        {
            var result = new ServiceResult<MedicineDTO>();

            try
            {
                var medicine = _mapper.Map<Medicine>(createMedicineDTO);
                var createdMedicine = await _unitOfWork.MedicineRepository.AddAsync(medicine);

                if (createdMedicine == null)
                {
                    result.Errors.Add("Failed to create medicine");
                    return result;
                }

                await _unitOfWork.Complete();
                result.Data = _mapper.Map<MedicineDTO>(createdMedicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medicine");
                result.Errors.Add("Failed to create medicine");
            }

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateMedicineAsync(int id, MedicineDTO medicineDTO)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var existingMedicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (existingMedicine == null)
                {
                    result.Errors.Add($"Medicine with ID {id} not found");
                    return result;
                }

                _mapper.Map(medicineDTO, existingMedicine);

                _unitOfWork.MedicineRepository.Update(existingMedicine);


                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medicine {MedicineId}", id);
                result.Errors.Add("Failed to update medicine");
            }

            return result;
        }
        public async Task<ServiceResult<bool>> UpdateMedicineStock(int id, int stock)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var existingMedicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (existingMedicine == null)
                {
                    result.Errors.Add($"Medicine with ID {id} not found");
                    return result;
                }

                existingMedicine.Stock = stock;

                _unitOfWork.MedicineRepository.Update(existingMedicine);


                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medicine {MedicineId}", id);
                result.Errors.Add("Failed to update medicine");
            }

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteMedicineAsync(int id)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (medicine == null)
                {
                    result.Errors.Add($"Medicine with ID {id} not found");
                    return result;
                }

                _unitOfWork.MedicineRepository.Delete(medicine);
                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medicine {MedicineId}", id);
                result.Errors.Add("Failed to delete medicine");
            }

            return result;
        }

        public async Task<ServiceResult<IEnumerable<MedicineDTO>>> GetLowStockMedicinesAsync()
        {
            var result = new ServiceResult<IEnumerable<MedicineDTO>>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetLowStockMedicinesAsync();
                result.Data = _mapper.Map<IEnumerable<MedicineDTO>>(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock medicines");
                result.Errors.Add("Failed to retrieve low stock medicines");
            }

            return result;
        }

        public async Task<ServiceResult<IEnumerable<MedicineDTO>>> GetMedicinesRequiringAuditAsync()
        {
            var result = new ServiceResult<IEnumerable<MedicineDTO>>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetMedicinesRequiringAuditAsync();
                result.Data = _mapper.Map<IEnumerable<MedicineDTO>>(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medicines requiring audit");
                result.Errors.Add("Failed to retrieve medicines requiring audit");
            }

            return result;
        }
    }
}

