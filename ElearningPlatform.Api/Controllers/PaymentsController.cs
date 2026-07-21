using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Contracts.Payments;
using ElearningPlatform.Application.Features.Payments.Commands.CreatePaymentIntent;
using ElearningPlatform.Infrastructure.Payments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IStripeWebhookService stripeWebhookService;

        public PaymentsController(IMediator mediator,IStripeWebhookService stripeWebhookService)
        {
            this.mediator = mediator;
            this.stripeWebhookService = stripeWebhookService;
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
        [HttpPost("webhook")]
        [AllowAnonymous]
        [SwaggerOperation(
    Summary = "Stripe webhook",
    Description = "Receives Stripe webhook events and updates payment and order status."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);

            var json = await reader.ReadToEndAsync();

            var signature = Request.Headers["Stripe-Signature"];

            await stripeWebhookService.HandleWebhookAsync(
                json,
                signature!);

            return Ok();
        }
    }
}
