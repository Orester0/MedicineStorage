using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class TokenService(IConfiguration _config, UserManager<User> _userManager) : ITokenService
    {
        public async Task<string> CreateToken(User user)
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
    }
}
