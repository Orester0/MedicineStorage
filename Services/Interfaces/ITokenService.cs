using MedicineStorage.Models;

namespace MedicineStorage.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);

    }
}
