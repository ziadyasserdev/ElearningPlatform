using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Reviews.Commands.CreateReview;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReviewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("courses/{courseId}/reviews")]
        [SwaggerOperation(
    Summary = "Create review",
    Description = "Creates a review and rating for the specified course."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateReview(
    int courseId,
    [FromBody] CreateReviewCommand command)
        {
            command.CourseId = courseId;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
