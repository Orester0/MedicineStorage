using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using System.Linq.Expressions;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRepository
    {
        Task<Medicine?> GetByIdAsync(int id);
        Task<List<Medicine>> GetAllAsync();
        Task<List<Medicine>> GetLowStockMedicinesAsync();
        Task<List<Medicine>> GetMedicinesRequiringAuditAsync();
        Task UpdateStockAsync(int medicineId, decimal quantity);
        Task<bool> CreateAsync(Medicine medicine);
        Task<bool> UpdateAsync(Medicine medicine);
        Task<bool> DeleteAsync(int id);
        Task<decimal> GetCurrentStockLevelAsync(int medicineId);
    }
}
