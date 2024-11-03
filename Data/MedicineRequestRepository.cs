using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class MedicineRequestRepository : IMedicineRequestRepository
    {
        public Task AddAsync(MedicineRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(MedicineRequest entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MedicineRequest>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MedicineRequest>> GetByConditionAsync(Expression<Func<MedicineRequest, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<MedicineRequest> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(MedicineRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
