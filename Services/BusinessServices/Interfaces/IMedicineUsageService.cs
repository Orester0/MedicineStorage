using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IMedicineUsageService
    {

        public Task<ServiceResult<ReturnMedicineUsageDTO>> CreateUsageAsync(
            CreateMedicineUsageDTO createUsageDTO,
            int userId);

        public Task<ServiceResult<ReturnMedicineUsageDTO>> GetUsageByIdAsync(int id);


        public Task<ServiceResult<List<ReturnMedicineUsageDTO>>> GetUsagesByUserIdAsync(int userId);

        public Task<ServiceResult<PagedList<ReturnMedicineUsageDTO>>> GetPaginatedUsages(MedicineUsageParams parameters);
    }
}
