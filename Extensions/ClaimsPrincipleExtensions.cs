using MedicineStorage.Models;
using System.Security.Claims;

namespace MedicineStorage.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static int GetUserIdFromClaims(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in token");
            }
            return userId;
        }

        public static List<string> GetUserRolesFromClaims(this ClaimsPrincipal user)
        {
            var roles = user.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();
            if (roles.Count == 0)
            {
                throw new UnauthorizedAccessException("No roles found for the user in the token");
            }
            return roles;
        }
    }
}
