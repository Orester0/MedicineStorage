using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IMedicineSupplyService
    {
        Task<ServiceResult<PagedList<ReturnMedicineSupplyDTO>>> GetPaginatedSupplies(MedicineSupplyParams parameters);
        Task<ServiceResult<MedicineSupply>> CreateSupplyByUserAsync(CreateMedicineSupplyDTO dto, int userId);
        Task<ServiceResult<MedicineSupply>> CreateSupplyForTenderAsync(int medicineId, int quantity, int tenderId);
    }
}
