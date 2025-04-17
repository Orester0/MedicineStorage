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
        IMapper _mapper,
        IMedicineSupplyService _medicineSupplyService) : IMedicineService
    {
        public async Task<ServiceResult<bool>> BulkCreateMedicinesAsync(List<BulkCreateMedicineDTO> bulkMedicines)
        {
            var result = new ServiceResult<bool>();

            foreach (var item in bulkMedicines)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    var medicine = _mapper.Map<Medicine>(item.Medicine);
                    medicine.LastAuditDate = null;
                    var category = await _unitOfWork.MedicineRepository.GetOrCreateCategoryAsync(item.Medicine.Category);
                    medicine.CategoryId = category.Id;
                    medicine.Stock = item.InitialStock;

                    // Add medicine and save first to get valid ID
                    var createdMedicine = await _unitOfWork.MedicineRepository.AddAsync(medicine);
                    await _unitOfWork.CompleteAsync();

                    // Now create supply with valid MedicineId
                    var supply = new MedicineSupply
                    {
                        MedicineId = createdMedicine.Id,
                        Quantity = item.InitialStock,
                        TransactionDate = DateTime.UtcNow
                    };
                    await _unitOfWork.MedicineSupplyRepository.AddAsync(supply);
                    await _unitOfWork.CompleteAsync();

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return result;
                }
            }

            result.Data = true;
            return result;
        }


        public async Task<ServiceResult<object>> GetMedicineReportAsync(int medicineId, DateTime startDate, DateTime endDate)
        {
            var result = new ServiceResult<object>();

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
            }

            var medicineDto = _mapper.Map<ReturnMedicineReportDTO>(medicine);

            var audits = await _unitOfWork.AuditRepository.GetByMedicineIdAndDateRangeAsync(medicineId, startDate, endDate);
            var auditDtos = audits
                .Select(audit =>
                {
                    var item = audit.AuditItems.FirstOrDefault(ai => ai.MedicineId == medicineId);
                    if (item == null) return null;

                    var report = _mapper.Map<ReturnAuditReportDTO>(audit);
                    report.ExpectedQuantity = item.ExpectedQuantity;
                    report.ActualQuantity = item.ActualQuantity;
                    report.CheckedByUser = _mapper.Map<ReturnUserGeneralDTO>(item.CheckedByUser);

                    return report;
                })
                .Where(dto => dto != null)
                .ToList();

            var tenders = await _unitOfWork.TenderRepository.GetByMedicineIdAndDateRangeAsync(medicineId, startDate, endDate);
            var tenderDtos = tenders
                .Select(tender =>
                {
                    var item = tender.TenderItems.FirstOrDefault(ti => ti.MedicineId == medicineId);
                    if (item == null) return null;

                    var report = _mapper.Map<ReturnTenderReportDTO>(tender);
                    report.RequiredQuantity = item.RequiredQuantity;

                    return report;
                })
                .Where(dto => dto != null)
                .ToList();

            var requests = await _unitOfWork.MedicineRequestRepository.GetByMedicineIdAndDateRangeAsync(
                medicineId, startDate, endDate);
            var requestDtos = _mapper.Map<List<ReturnMedicineRequestReportDTO>>(requests);

            var report = new
            {
                Medicine = medicineDto,
                DateRange = new { StartDate = startDate, EndDate = endDate },
                Audits = auditDtos,
                Tenders = tenderDtos,
                Requests = requestDtos
            };

            result.Data = report;
            return result;
        }

        public async Task<ServiceResult<List<string>>> GetAllCategoriesAsync()
        {
            var result = new ServiceResult<List<string>>();
            var categories = await _unitOfWork.MedicineRepository.GetAllCategoriesAsync();
            result.Data = categories.Select(c => c.Name).ToList();
            return result;
        }

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

            var category = await _unitOfWork.MedicineRepository.GetOrCreateCategoryAsync(createMedicineDTO.Category);
            medicine.CategoryId = category.Id;

            var created = await _unitOfWork.MedicineRepository.AddAsync(medicine);
            await _unitOfWork.CompleteAsync();

            result.Data = _mapper.Map<ReturnMedicineDTO>(created);
            return result;
        }


        public async Task<ServiceResult<bool>> UpdateMedicineAsync(int medicineId, UpdateMedicineDTO dto)
        {
            var result = new ServiceResult<bool>();
            var existing = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
            if (existing == null) throw new KeyNotFoundException();

            var previousCategoryId = existing.CategoryId;

            _mapper.Map(dto, existing);

            var newCategory = await _unitOfWork.MedicineRepository.GetOrCreateCategoryAsync(dto.Category);
            existing.CategoryId = newCategory.Id;

            _unitOfWork.MedicineRepository.Update(existing);
            await _unitOfWork.CompleteAsync();

            if (previousCategoryId != newCategory.Id &&
                await _unitOfWork.MedicineRepository.IsCategoryUnusedAsync(previousCategoryId))
            {
                await _unitOfWork.MedicineRepository.DeleteCategoryAsync(previousCategoryId);
            }

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

            var categoryId = medicine.CategoryId;

            await _unitOfWork.MedicineRepository.DeleteAsync(medicine.Id);
            await _unitOfWork.CompleteAsync();

            if (await _unitOfWork.MedicineRepository.IsCategoryUnusedAsync(categoryId))
            {
                await _unitOfWork.MedicineRepository.DeleteCategoryAsync(categoryId);
            }

            result.Data = true;
            return result;
        }

    }
}

