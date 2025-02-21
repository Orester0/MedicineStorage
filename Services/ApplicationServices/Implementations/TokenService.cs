using MedicineStorage.Data;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class TokenService(IConfiguration _config, UserManager<User> _userManager, AppDbContext _context) : ITokenService
    {
        public async Task<string> CreateAccessToken(User user)
        {
            var tokenKey = _config["TokenKey"] ?? throw new Exception("Cannot access token key");

            if (tokenKey.Length < 64)
            {
                throw new Exception("Token key needs to be longer");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            if (user.UserName == null)
            {
                throw new Exception("No UserName for user");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> CreateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<ReturnUserTokenDTO> RefreshAccessToken(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (storedToken.ExpiryDate < DateTime.UtcNow || storedToken.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token expired or revoked");

            var newAccessToken = await CreateAccessToken(storedToken.User);
            var newRefreshToken = await CreateRefreshToken(storedToken.User);

            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            return new ReturnUserTokenDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeRefreshToken(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (storedToken != null)
            {
                storedToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
