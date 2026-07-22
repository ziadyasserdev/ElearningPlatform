using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Reviews.Commands.ApproveReview;
using ElearningPlatform.Application.Features.Reviews.Commands.CreateReview;
using ElearningPlatform.Application.Features.Reviews.Commands.DeleteReview;
using ElearningPlatform.Application.Features.Reviews.Commands.DeleteReviewByAdmin;
using ElearningPlatform.Application.Features.Reviews.Commands.RejectReview;
using ElearningPlatform.Application.Features.Reviews.Commands.UpdateReview;
using ElearningPlatform.Application.Features.Reviews.Queries.GetAllReviews;
using ElearningPlatform.Application.Features.Reviews.Queries.GetCourseReviews;
using ElearningPlatform.Application.Features.Reviews.Queries.GetCourseReviewSummary;
using ElearningPlatform.Application.Features.Reviews.Queries.GetMyReviews;
using ElearningPlatform.Application.Features.Reviews.Queries.GetPendingReviews;
using ElearningPlatform.Application.Features.Reviews.Queries.GetReviewStatistics;
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
        [HttpGet("courses/{courseId}/reviews")]
        [SwaggerOperation(
    Summary = "Get course reviews",
    Description = "Retrieves a paginated list of reviews for the specified course."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseReviews(
    int courseId,
    [FromQuery] GetCourseReviewsQuery query)
        {
            query.CourseId = courseId;

            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("courses/{courseId}/reviews/summary")]
        [SwaggerOperation(
    Summary = "Get course review summary",
    Description = "Retrieves the review summary and rating statistics for the specified course."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseReviewSummary(int courseId)
        {
            var result = await mediator.Send(new GetCourseReviewSummaryQuery(courseId));
            return result.ToActionResult();
        }
        [HttpPatch("{id}/approve")]
        [SwaggerOperation(
    Summary = "Approve review",
    Description = "Approves the specified review."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveReview(int id)
        {
            var result = await mediator.Send(new ApproveReviewCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("{id}/reject")]
        [SwaggerOperation(
    Summary = "Reject review",
    Description = "Rejects the specified review with a rejection reason."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectReview(
    int id,
    [FromBody] RejectReviewCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpGet("pending")]
        [SwaggerOperation(
    Summary = "Get pending reviews",
    Description = "Retrieves a paginated list of pending reviews with optional filtering."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPendingReviews(
    [FromQuery] GetPendingReviewsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get all reviews",
    Description = "Retrieves a paginated list of reviews with optional filtering by status, rating, date range, and search keyword."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllReviews(
    [FromQuery] GetAllReviewsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("statistics")]
        [SwaggerOperation(
    Summary = "Get review statistics",
    Description = "Retrieves overall review statistics."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReviewStatistics()
        {
            var result = await mediator.Send(new GetReviewStatisticsQuery());
            return result.ToActionResult();
        }
        [HttpDelete("{id}/admin")]
        [SwaggerOperation(
    Summary = "Delete review by admin",
    Description = "Deletes the specified review by an administrator."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReviewByAdmin(int id)
        {
            var result = await mediator.Send(new DeleteReviewByAdminCommand(id));
            return result.ToActionResult();
        }
    }
}
