using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);

    }
}
