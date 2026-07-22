using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Reviews.Commands.CreateReview;
using ElearningPlatform.Application.Features.Reviews.Commands.DeleteReview;
using ElearningPlatform.Application.Features.Reviews.Commands.UpdateReview;
using ElearningPlatform.Application.Features.Reviews.Queries.GetMyReviews;
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
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Update review",
    Description = "Updates an existing review."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReview(
    int id,
    [FromBody] UpdateReviewCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete review",
    Description = "Deletes the specified review."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await mediator.Send(new DeleteReviewCommand(id));
            return result.ToActionResult();
        }
        [HttpGet("my-reviews")]
        [SwaggerOperation(
    Summary = "Get my reviews",
    Description = "Retrieves a paginated list of the authenticated user's reviews."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyReviews(
    [FromQuery] GetMyReviewsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
    }
}
