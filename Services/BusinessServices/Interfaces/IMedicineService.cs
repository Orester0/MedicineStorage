using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IMedicineService
    {

        Task<ServiceResult<object>> GetMedicineReportAsync(int medicineId, DateTime startDate, DateTime endDate);
        Task<ServiceResult<List<string>>> GetAllCategoriesAsync();
        Task<ServiceResult<PagedList<ReturnMedicineDTO>>> GetPaginatedMedicines(MedicineParams parameters);
        Task<ServiceResult<List<ReturnMedicineDTO>>> GetAllMedicinesAsync();
        Task<ServiceResult<List<MedicineStockForecastDTO>>> GetMedicineStockForecast(bool considerRequests = false, bool considerTenders = false);
        Task<ServiceResult<ReturnMedicineDTO>> GetMedicineByIdAsync(int id);
        Task<ServiceResult<ReturnMedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO);
        Task<ServiceResult<bool>> UpdateMedicineAsync(int id, UpdateMedicineDTO medicineDTO);
        Task<ServiceResult<bool>> DeleteMedicineAsync(int id, List<string> userRoles);
    }
}
