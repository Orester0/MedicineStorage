using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicineStorage.Images
{
    public class MedicineService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper) : IMedicineService
    {

        public async Task<ServiceResult<MedicineAuditAndTenderDto>> GetProblematicMedicinesAsync()
        {

            var result = new ServiceResult<MedicineAuditAndTenderDto>();
            var medicinesNeedingAudit = await _unitOfWork.MedicineRepository.GetMedicinesNeedingAuditAsync();
            var medicinesNeedingTender = await _unitOfWork.MedicineRepository.GetMedicinesNeedingTenderAsync();

            MedicineAuditAndTenderDto dto = new MedicineAuditAndTenderDto
            {
                MedicinesNeedingAudit = _mapper.Map<List<ReturnMedicineShortDTO>>(medicinesNeedingAudit),
                MedicinesNeedingTender = _mapper.Map<List<ReturnMedicineShortDTO>>(medicinesNeedingTender)
            };
            result.Data = dto;

            return result;
        }


        public async Task<ServiceResult<bool>> UpdateMinimumStockForecastForAllMedicinesAsync(int forecastDays = 30)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
                DateTime startDate = DateTime.UtcNow.AddMonths(-3); 
                DateTime endDate = DateTime.UtcNow;

                var allMedicineRequests = await _unitOfWork.MedicineRequestRepository.GetAllMedicineRequestsInDateRangeAsync(
                    startDate, endDate);

                var allSupplies = await _unitOfWork.MedicineSupplyRepository.GetAllMedicineSuppliesInDateRangeAsync(
                    startDate, endDate);

                foreach (var medicine in medicines)
                {
                    var requests = allMedicineRequests.ContainsKey(medicine.Id)
                        ? allMedicineRequests[medicine.Id]
                        : new List<MedicineRequest>();

                    var requestsQuantity = requests
                        .Where(r => r.Status == RequestStatus.Approved)
                        .Sum(r => r.Quantity);

                    var supplies = allSupplies.ContainsKey(medicine.Id)
                        ? allSupplies[medicine.Id]
                        : new List<MedicineSupply>();
                    var suppliesQuantity = supplies.Sum(s => s.Quantity);

                    double daysInPeriod = (endDate - startDate).TotalDays;
                    double dailyConsumptionRate = daysInPeriod > 0 ? (double)requestsQuantity / daysInPeriod : 0;

                    double forecastedConsumption = dailyConsumptionRate * forecastDays;

                    double safetyBuffer = 1.5;
                    int newMinimumStock = (int)Math.Ceiling(forecastedConsumption * safetyBuffer);

                    newMinimumStock = Math.Max(1, newMinimumStock);

                    medicine.MinimumStock = newMinimumStock;
                    _unitOfWork.MedicineRepository.Update(medicine);
                }

                await _unitOfWork.CompleteAsync();
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Data = false;
            }

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
            //var nameParam = new SqlParameter("@Name", SqlDbType.NVarChar, 255)
            //{
            //    Value = createMedicineDTO.Category
            //};
            //var categoryIdParam = new SqlParameter("@Id", SqlDbType.Int)
            //{
            //    Direction = ParameterDirection.Output
            //};

            //await _unitOfWork.ExecuteStoredProcedureAsync("sp_GetOrInsertCategory",
            //    new[]
            //    {
            //        nameParam,
            //        categoryIdParam
            //    }
            //);

            //medicine.CategoryId =  categoryIdParam.Value != DBNull.Value
            //    ? Convert.ToInt32(categoryIdParam.Value)
            //    : throw new InvalidOperationException("Failed to get or create category ID");


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

