using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Data
{
    public class MedicineUsageRepository : IMedicineUsageRepository
    {
        public Task AddAsync(MedicineUsage entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(MedicineUsage entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MedicineUsage>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MedicineUsage> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(MedicineUsage entity)
        {
            throw new NotImplementedException();
        }
    }
}
