using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Coupons.Commands.ChangeCouponStatus;
using ElearningPlatform.Application.Features.Coupons.Commands.CreateCoupon;
using ElearningPlatform.Application.Features.Coupons.Commands.DeleteCoupon;
using ElearningPlatform.Application.Features.Coupons.Commands.UpdateCoupon;
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
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Update coupon",
    Description = "Updates an existing coupon."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCouponCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete coupon",
    Description = "Deletes a coupon."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteCouponCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("{id}/status")]
        [SwaggerOperation(
    Summary = "Change coupon status",
    Description = "Activates or deactivates a coupon."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus(
    int id,
    [FromBody] ChangeCouponStatusCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
