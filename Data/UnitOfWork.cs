using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository => throw new NotImplementedException();

        public IMedicineRequestRepository MedicineRequestRepository => throw new NotImplementedException();
        public IMedicineRepository MedicineRepository => throw new NotImplementedException();

        public IStockRepository StockRepository => throw new NotImplementedException();


        public Task<bool> Complete()
        {
            throw new NotImplementedException();
        }

        public bool HasChanges()
        {
            throw new NotImplementedException();
        }
    }
}
