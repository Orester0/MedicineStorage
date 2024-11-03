using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Interfaces;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class TenderRepository : ITenderRepository
    {
        public Task AddAsync(Tender entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Tender entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tender>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tender> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Tender entity)
        {
            throw new NotImplementedException();
        }
    }
}
