using MediatR;
using MedicineStorage.Models.Medicine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicineRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<MedicineRequest>> CreateRequest(CreateMedicineRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ApproveRequest(int id, ApproveMedicineRequestCommand command)
        {
            command.RequestId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("monthly")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<MedicineRequest>>> GetMonthlyRequests([FromQuery] DateTime month)
        {
            var query = new GetMonthlyRequestsQuery { Month = month };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
