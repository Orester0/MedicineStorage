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
        IMapper _mapper) : IMedicineService
    {
        public async Task<ServiceResult<IEnumerable<ReturnMedicineDTO>>> GetAllMedicinesAsync()
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineDTO>>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineDTO>>(medicines);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve medicines");
            }

            return result;
        }

        public async Task<ServiceResult<ReturnMedicineDTO>> GetMedicineByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnMedicineDTO>();

            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (medicine == null)
                {
                    result.Errors.Add($"Medicine with ID {id} not found");
                    return result;
                }

                result.Data = _mapper.Map<ReturnMedicineDTO>(medicine);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve medicine");
            }

            return result;
        }

        public async Task<ServiceResult<ReturnMedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO)
        {
            var result = new ServiceResult<ReturnMedicineDTO>();

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
                result.Data = _mapper.Map<ReturnMedicineDTO>(createdMedicine);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to create medicine");
            }

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateMedicineAsync(int id, CreateMedicineDTO medicineDTO)
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
                result.Errors.Add("Failed to delete medicine");
            }

            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineDTO>>> GetLowStockMedicinesAsync()
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineDTO>>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetLowStockMedicinesAsync();
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineDTO>>(medicines);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve low stock medicines");
            }

            return result;
        }

        public async Task<ServiceResult<IEnumerable<ReturnMedicineDTO>>> GetMedicinesRequiringAuditAsync()
        {
            var result = new ServiceResult<IEnumerable<ReturnMedicineDTO>>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetMedicinesRequiringAuditAsync();
                result.Data = _mapper.Map<IEnumerable<ReturnMedicineDTO>>(medicines);
            }
            catch (Exception ex)
            {
                result.Errors.Add("Failed to retrieve medicines requiring audit");
            }

            return result;
        }
    }
}

