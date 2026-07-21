using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Payments.Commands.CreatePaymentIntent;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PaymentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("create-payment-intent")]
        [SwaggerOperation(
    Summary = "Create payment intent",
    Description = "Creates a Stripe payment intent for a pending order."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreatePaymentIntent(
    [FromBody] CreatePaymentIntentCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
