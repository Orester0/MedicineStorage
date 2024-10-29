using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Audit>> CreateAudit(CreateAuditCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("scheduled")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<AuditSchedule>>> GetScheduledAudits()
        {
            var result = await _mediator.Send(new GetScheduledAuditsQuery());
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<Audit>>> GetDoctorAudits(int doctorId)
        {
            var query = new GetDoctorAuditsQuery { DoctorId = doctorId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
