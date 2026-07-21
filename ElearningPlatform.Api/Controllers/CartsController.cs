using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Carts.Commands.AddCourseToCart;
using ElearningPlatform.Application.Features.Carts.Queries.GetMyCart;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CartsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("add-course")]
        [SwaggerOperation(
    Summary = "Add course to cart",
    Description = "Add a course to the authenticated user's shopping cart."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddCourseToCart([FromBody] AddCourseToCartCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpGet("my-cart")]
        [SwaggerOperation(
    Summary = "Get my cart",
    Description = "Retrieve the authenticated user's shopping cart with all added courses."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyCart()
        {
            var result = await mediator.Send(new GetMyCartQuery());
            return result.ToActionResult();
        }
    }
}
