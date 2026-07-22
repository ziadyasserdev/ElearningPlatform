using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Orders.Commands.CancelOrder;
using ElearningPlatform.Application.Features.Orders.Commands.Checkout;
using ElearningPlatform.Application.Features.Orders.Queries.GetMyOrders;
using ElearningPlatform.Application.Features.Orders.Queries.GetOrderDetails;
using ElearningPlatform.Application.Features.Orders.Queries.GetOrders;
using ElearningPlatform.Application.Features.Orders.Queries.GetOrderStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{orderId}")]
        [SwaggerOperation(
    Summary = "Get order details",
    Description = "Retrieves the details of a specific order for the authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await mediator.Send(new GetOrderDetailsQuery(orderId));
            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get orders",
    Description = "Retrieves a paginated list of orders with optional search and status filtering."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOrders(
    [FromQuery] GetOrdersQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpPost("{orderId}/cancel")]
        [SwaggerOperation(
    Summary = "Cancel order",
    Description = "Cancels a pending order."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await mediator.Send(
                new CancelOrderCommand(orderId));

            return result.ToActionResult();
        }
        [HttpGet("statistics")]
      
        [SwaggerOperation(
    Summary = "Order statistics",
    Description = "Returns order statistics for the admin dashboard."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await mediator.Send(new GetOrderStatisticsQuery());

            return result.ToActionResult();
        }
    }
}
