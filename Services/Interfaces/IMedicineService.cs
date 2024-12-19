using MedicineStorage.DTOs;
using MedicineStorage.Helpers.Params;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface IMedicineService
    {
        public Task<ServiceResult<PagedList<ReturnMedicineDTO>>> GetMedicinesAsync(MedicineParams parameters);
        public Task<ServiceResult<ReturnMedicineDTO>> GetMedicineByIdAsync(int id);
        public Task<ServiceResult<ReturnMedicineDTO>> CreateMedicineAsync(CreateMedicineDTO createMedicineDTO);
        public Task<ServiceResult<bool>> UpdateMedicineAsync(int id, CreateMedicineDTO medicineDTO);
        public Task<ServiceResult<bool>> DeleteMedicineAsync(int id);
    }
}
