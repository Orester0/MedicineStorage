using MedicineStorage.DTOs;
using MedicineStorage.Models;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineService
    {
        public Task<ServiceResult<IEnumerable<MedicineDTO>>> GetAllMedicinesAsync();

        public Task<ServiceResult<MedicineDTO>> GetMedicineByIdAsync(int id);
        public Task<ServiceResult<MedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO);

        public Task<ServiceResult<bool>> UpdateMedicineAsync(int id, MedicineDTO medicineDTO);
        public Task<ServiceResult<bool>> DeleteMedicineAsync(int id);

        public Task<ServiceResult<IEnumerable<MedicineDTO>>> GetLowStockMedicinesAsync();

        public Task<ServiceResult<IEnumerable<MedicineDTO>>> GetMedicinesRequiringAuditAsync();
    }
}
