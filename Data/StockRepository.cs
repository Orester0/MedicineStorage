using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Interfaces;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class StockRepository : IStockRepository
    {
        public Task AddAsync(Stock entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Stock entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stock>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Stock> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Stock entity)
        {
            throw new NotImplementedException();
        }
    }
}
