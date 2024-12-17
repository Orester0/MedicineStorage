using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRepository
    {

        public Task<Medicine?> GetByIdAsync(int id);

        public Task<IEnumerable<Medicine>> GetAllAsync();
        public Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync();

        public Task<IEnumerable<Medicine>> GetMedicinesRequiringAuditAsync();

        public Task<Medicine?> AddAsync(Medicine medicine);

        public void Update(Medicine medicine);

        public void Delete(Medicine medicine);
    }
}
