using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands;
using Orders.Core.Entities;

namespace dispatch_order_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FulfillmentController : ControllerBase
    {
        private readonly ILogger<FulfillmentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public FulfillmentController(ILogger<FulfillmentController> logger, IConfiguration configuration, IMediator mediator)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("[action]", Name = "dispatch")]
        public async Task<ActionResult> dispatch([FromBody] OrderPayload orders)
        {
            OrderDispatchCommand orderDispatchCommand = new OrderDispatchCommand(orders);
            var result = await _mediator.Send(orderDispatchCommand);
            if (result)
            {
                return Ok("Order Processed!");
            }
            else
            {
                FailedDispatchCommand failedDispatchCommand = new FailedDispatchCommand(orders);
                var persistResult = await _mediator.Send(failedDispatchCommand);
                return BadRequest("Processing failed !");
            }
        }
    }
}
