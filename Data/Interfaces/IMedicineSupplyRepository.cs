using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.Tender;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineSupplyRepository
    {
        public Task<IEnumerable<MedicineSupply>> GetAllAsync();

        public Task<MedicineSupply> GetByIdAsync(int id);

        public Task<MedicineSupply> CreateMedicineSupplyAsync(MedicineSupply medicineSupply);

        public void UpdateMedicineSupply(MedicineSupply medicineSupply);

        public Task DeleteMedicineSupplyAsync(int id);
    }
}
