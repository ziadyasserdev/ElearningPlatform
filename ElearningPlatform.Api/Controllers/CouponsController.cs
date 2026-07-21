using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Coupons.Commands.CreateCoupon;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CouponsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
    Summary = "Create coupon",
    Description = "Creates a new coupon."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateCouponCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
