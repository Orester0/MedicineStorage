using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IMedicineUsageService
    {

        public Task<ServiceResult<ReturnMedicineUsageDTO>> CreateUsageAsync(
            CreateMedicineUsageDTO createUsageDTO,
            int userId);

        public Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsageByIdAsync(int id);


        public Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsagesByUserIdAsync(int userId);

        public Task<ServiceResult<PagedList<ReturnMedicineUsageDTO>>> GetAllUsagesAsync(MedicineUsageParams parameters);
    }
}
