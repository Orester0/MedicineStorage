using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class MedicineRepository : IMedicineRepository
    {
        public Task AddAsync(Medicine entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Medicine entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Medicine>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Medicine> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Medicine entity)
        {
            throw new NotImplementedException();
        }
    }
}
