using MediatR;
using MedicineStorage.Models.Medicine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineUsageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicineUsageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Nurse")]
        public async Task<ActionResult> RecordUsage(RecordMedicineUsageCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<List<MedicineUsage>>> GetDoctorUsage(
            int doctorId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetDoctorUsageQuery
            {
                DoctorId = doctorId,
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
