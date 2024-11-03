using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMedicineRequestRepository MedicineRequestRepository { get; }
        IMedicineRepository MedicineRepository { get; }
        
        IStockRepository StockRepository { get; }

        Task<bool> Complete();

        bool HasChanges();

    }
}
