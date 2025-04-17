using MedicineStorage.Controllers.Interface;
using MedicineStorage.Extensions;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers.Implementation
{

    [Authorize]
    [Route("api/medicine-supply")]
    public class MedicineSupplyController(IMedicineSupplyService _medicineSupplyService) : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetSupplies([FromQuery] MedicineSupplyParams parameters)
        {
            var result = await _medicineSupplyService.GetPaginatedSupplies(parameters);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> CreateSupply([FromBody] CreateMedicineSupplyDTO createSupplyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetUserIdFromClaims();
            var result = await _medicineSupplyService.CreateSupplyByUserAsync(createSupplyDTO, userId);
            if (!result.Success)
            {
                return BadRequest(new { result.Errors });
            }
            return Ok(result.Data);
        }

    }
}
