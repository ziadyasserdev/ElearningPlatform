using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Orders.Commands.Checkout;
using ElearningPlatform.Application.Features.Orders.Queries.GetMyOrders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("checkout")]
        [SwaggerOperation(
    Summary = "Checkout",
    Description = "Creates a new order from the authenticated user's shopping cart."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Checkout()
        {
            var result = await mediator.Send(new CheckoutCommand());
            return result.ToActionResult();
        }
        [HttpGet("my-orders")]
        [SwaggerOperation(
    Summary = "Get my orders",
    Description = "Retrieves all orders for the authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = await mediator.Send(new GetMyOrdersQuery());
            return result.ToActionResult();
        }
    }
}
