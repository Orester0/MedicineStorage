using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateAccessToken(User user);
        Task<string> CreateRefreshToken(User user);
        Task<ReturnUserTokenDTO> RefreshAccessToken(string refreshToken);
        Task RevokeRefreshToken(string refreshToken);

    }
}
