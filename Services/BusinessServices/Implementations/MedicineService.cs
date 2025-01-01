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
    public class MedicineService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper) : IMedicineService
    {
        public async Task<ServiceResult<PagedList<DTOs.ReturnMedicineDTO>>> GetMedicinesAsync(MedicineParams parameters)
        {
            var result = new ServiceResult<PagedList<DTOs.ReturnMedicineDTO>>();

            try
            {
                var (medicines, totalCount) = await _unitOfWork.MedicineRepository.GetAllAsync(parameters);
                var dtos = _mapper.Map<IEnumerable<DTOs.ReturnMedicineDTO>>(medicines);
                result.Data = new PagedList<DTOs.ReturnMedicineDTO>(dtos.ToList(), totalCount, parameters.PageNumber, parameters.PageSize);
            }
            catch (Exception)
            {
                result.Errors.Add("Failed to retrieve medicines");
            }

            return result;
        }

        public async Task<ServiceResult<DTOs.ReturnMedicineDTO>> GetMedicineByIdAsync(int id)
        {
            var result = new ServiceResult<DTOs.ReturnMedicineDTO>();

            try
            {
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
                if (medicine == null)
                {
                    result.Errors.Add($"ReturnMedicineDTO with ID {id} not found");
                    return result;
                }

                result.Data = _mapper.Map<DTOs.ReturnMedicineDTO>(medicine);
            }
            catch (Exception)
            {
                result.Errors.Add("Failed to retrieve medicine");
            }

            return result;
        }





        public async Task<ServiceResult<DTOs.ReturnMedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO)
        {
            var result = new ServiceResult<DTOs.ReturnMedicineDTO>();

            try
            {
                var medicine = _mapper.Map<Medicine>(createMedicineDTO);
                var createdMedicine = await _unitOfWork.MedicineRepository.CreateMedicineAsync(medicine);

                if (createdMedicine == null)
                {
                    result.Errors.Add("Failed to create medicine");
                    return result;
                }

                await _unitOfWork.Complete();
                result.Data = _mapper.Map<DTOs.ReturnMedicineDTO>(createdMedicine);
            }
            catch (Exception)
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
                    result.Errors.Add($"ReturnMedicineDTO with ID {id} not found");
                    return result;
                }

                _mapper.Map(medicineDTO, existingMedicine);

                _unitOfWork.MedicineRepository.UpdateMedicine(existingMedicine);


                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception)
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
                    result.Errors.Add($"ReturnMedicineDTO with ID {id} not found");
                    return result;
                }

                _unitOfWork.MedicineRepository.DeleteMedicine(medicine);
                await _unitOfWork.Complete();

                result.Data = true;
            }
            catch (Exception)
            {
                result.Errors.Add("Failed to delete medicine");
            }

            return result;
        }

    }
}

