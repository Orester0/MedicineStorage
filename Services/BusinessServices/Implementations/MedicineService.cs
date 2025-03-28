using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class MedicineService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper) : IMedicineService
    {
        public async Task<ServiceResult<List<ReturnMedicineDTO>>> GetAllMedicinesAsync()
        {
            var result = new ServiceResult<List<ReturnMedicineDTO>>();
            var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
            var dtos = _mapper.Map<List<ReturnMedicineDTO>>(medicines);
            result.Data = dtos;
            return result;
        }

        public async Task<ServiceResult<List<MedicineStockForecastDTO>>> GetMedicineStockForecast(
            bool considerRequests = false,
            bool considerTenders = false)
        {
            var result = new ServiceResult<List<MedicineStockForecastDTO>>();
            var forecastList = new List<MedicineStockForecastDTO>();

            var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();

            foreach (var medicine in medicines)
            {
                int totalStock = (int)medicine.Stock;
                int totalTenderStock = 0;
                int totalRequested = 0;

                if (considerTenders)
                {
                    var relevantTenders = await _unitOfWork.TenderRepository.GetRelevantTendersAsync();

                    var tenderItems = relevantTenders
                        .SelectMany(t => t.TenderItems)
                        .Where(ti => ti.MedicineId == medicine.Id);

                    totalTenderStock = (int)tenderItems.Sum(ti => ti.RequiredQuantity);
                }


                if (considerRequests)
                {
                    var medicineRequests = await _unitOfWork.MedicineRequestRepository.GetByMedicineIdAsync(medicine.Id);
                    totalRequested = (int)medicineRequests.Sum(r => r.Quantity);
                }

                int projectedStock = totalStock + totalTenderStock - totalRequested;
                bool needsRestock = projectedStock < medicine.MinimumStock;

                forecastList.Add(new MedicineStockForecastDTO
                {
                    Medicine = _mapper.Map<ReturnMedicineDTO>(medicine),
                    CurrentStock = totalStock,
                    TenderStock = totalTenderStock,
                    RequestedAmount = totalRequested,
                    ProjectedStock = projectedStock,
                    NeedsRestock = needsRestock
                });
            }

            result.Data = forecastList;
            return result;
        }


        public async Task<ServiceResult<PagedList<ReturnMedicineDTO>>> GetPaginatedMedicines(MedicineParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnMedicineDTO>>();
            var (medicines, totalCount) = await _unitOfWork.MedicineRepository.GetByParams(parameters);
            var dtos = _mapper.Map<IEnumerable<ReturnMedicineDTO>>(medicines);
            result.Data = new PagedList<ReturnMedicineDTO>(
                dtos.ToList(), 
                totalCount, 
                parameters.PageNumber, 
                parameters.PageSize);

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

        /////////////////////////////////////////////////////////////////////////////////////

        public async Task<ServiceResult<ReturnMedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO)
        {
            var result = new ServiceResult<ReturnMedicineDTO>();
            var medicine = _mapper.Map<Medicine>(createMedicineDTO);
            medicine.LastAuditDate = null;
            var createdMedicine = await _unitOfWork.MedicineRepository.AddAsync(medicine);

            await _unitOfWork.CompleteAsync();
            result.Data = _mapper.Map<ReturnMedicineDTO>(createdMedicine);
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateMedicineAsync(int medicineId, UpdateMedicineDTO medicineDTO)
        {
            var result = new ServiceResult<bool>();
            var existingMedicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
            if (existingMedicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
            }

            _mapper.Map(medicineDTO, existingMedicine);
            _unitOfWork.MedicineRepository.Update(existingMedicine);
            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteMedicineAsync(int medicineId, List<string> userRoles)
        {
            var result = new ServiceResult<bool>();
            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
            }

            await _unitOfWork.MedicineRepository.DeleteAsync(medicine.Id);
            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }
    }
}

