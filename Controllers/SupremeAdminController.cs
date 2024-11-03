using AutoMapper;
using MedicineStorage.Models;
using MedicineStorage.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    [Authorize(Policy = "SupremeAdmin")]
    public class SupremeAdminController(UserManager<User> _userManager, RoleManager<AppRole> _roleManager, ITokenService _tokenService, IMapper _mapper) : BaseApiController
    {
        
        [HttpPut("update-roles/{userId}")]
        public async Task<IActionResult> UpdateUserRoles(int userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Role '{role}' does not exist.");
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return BadRequest(removeResult.Errors);
            }

            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
            {
                return BadRequest(addResult.Errors);
            }

            return Ok("User roles updated successfully.");
        }

    }
}
