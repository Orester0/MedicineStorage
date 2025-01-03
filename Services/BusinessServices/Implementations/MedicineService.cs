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
        public async Task<ServiceResult<PagedList<ReturnMedicineDTO>>> GetMedicinesAsync(MedicineParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnMedicineDTO>>();
                var (medicines, totalCount) = await _unitOfWork.MedicineRepository.GetAllAsync(parameters);
                var dtos = _mapper.Map<IEnumerable<ReturnMedicineDTO>>(medicines);
                result.Data = new PagedList<ReturnMedicineDTO>(dtos.ToList(), totalCount, parameters.PageNumber, parameters.PageSize);

            return result;
        }

        public async Task<ServiceResult<ReturnMedicineDTO>> GetMedicineByIdAsync(int medicineId)
        {
            var result = new ServiceResult<ReturnMedicineDTO>();
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                if (medicine == null)
                {

                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
                }

                result.Data = _mapper.Map<ReturnMedicineDTO>(medicine);

            return result;
        }





        public async Task<ServiceResult<ReturnMedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO)
        {
            var result = new ServiceResult<ReturnMedicineDTO>();
                var medicine = _mapper.Map<Medicine>(createMedicineDTO);
                var createdMedicine = await _unitOfWork.MedicineRepository.CreateMedicineAsync(medicine);

                await _unitOfWork.Complete();
                result.Data = _mapper.Map<ReturnMedicineDTO>(createdMedicine);

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateMedicineAsync(int medicineId, CreateMedicineDTO medicineDTO)
        {
            var result = new ServiceResult<bool>();
                var existingMedicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                if (existingMedicine == null)
                {

                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
                }

                _mapper.Map(medicineDTO, existingMedicine);

                _unitOfWork.MedicineRepository.UpdateMedicine(existingMedicine);


                await _unitOfWork.Complete();

                result.Data = true;

            return result;
        }
        public async Task<ServiceResult<bool>> DeleteMedicineAsync(int medicineId)
        {
            var result = new ServiceResult<bool>();
                var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
                if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
                return result;
                }

                _unitOfWork.MedicineRepository.DeleteMedicine(medicine);
                await _unitOfWork.Complete();

                result.Data = true;

            return result;
        }

    }
}

