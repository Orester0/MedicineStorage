using MediatR;
using MedicineStorage.Models.Tender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Tender>> CreateTender(CreateTenderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/bid")]
        [Authorize(Roles = "Distributor")]
        public async Task<ActionResult> SubmitBid(int id, SubmitBidCommand command)
        {
            command.TenderId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("active")]
        [Authorize(Roles = "Distributor")]
        public async Task<ActionResult<List<Tender>>> GetActiveTenders()
        {
            var result = await _mediator.Send(new GetActiveTendersQuery());
            return Ok(result);
        }
    }
}
