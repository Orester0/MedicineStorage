using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicineStorage.Controllers.Interface
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in token");
            }
            return userId;
        }
        protected List<string> GetUserRolesFromClaims()
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();
            if (roles.Count == 0)
            {
                throw new UnauthorizedAccessException("No roles found for the user in the token");
            }
            return roles;
        }

    }
}
