using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Submissions.Commands.CreateSubmission;
using ElearningPlatform.Application.Features.Submissions.Commands.DeleteSubmission;
using ElearningPlatform.Application.Features.Submissions.Commands.GradeSubmission;
using ElearningPlatform.Application.Features.Submissions.Commands.ReplaceSubmission;
using ElearningPlatform.Application.Features.Submissions.Queries.GetMySubmission;
using ElearningPlatform.Application.Features.Submissions.Queries.GetMySubmissions;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubmissionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("{assignmentId:int}/submissions")]
        [SwaggerOperation(
    Summary = "Create submission",
    Description = "Creates a new submission for the specified assignment."
)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateSubmission(
    int assignmentId,
    [FromForm] CreateSubmissionCommand command)
        {
            command = command with { AssignmentId = assignmentId };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("{assignmentId:int}/submissions/me")]
        [SwaggerOperation(
    Summary = "Get my submission",
    Description = "Retrieves the current student's submission for the specified assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMySubmission(int assignmentId)
        {
            var query = new GetMySubmissionQuery(assignmentId);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("my-submissions")]
        [SwaggerOperation(
    Summary = "Get my submissions",
    Description = "Retrieves a paginated list of the current student's submissions with optional filtering and sorting."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMySubmissions(
    [FromQuery] string? search = null,
    [FromQuery] SubmissionStatus? status = null,
    [FromQuery] bool? isLate = null,
    [FromQuery] SubmissionSortBy sortBy = SubmissionSortBy.SubmittedAt,
    [FromQuery] bool descending = true,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = new GetMySubmissionsQuery(
                search,
                status,
                isLate,
                sortBy,
                descending,
                pageNumber,
                pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpPut("{submissionId:int}")]
        [SwaggerOperation(
    Summary = "Replace submission",
    Description = "Replaces the uploaded file and optionally updates the submission comment."
)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReplaceSubmission(
    int submissionId,
    [FromForm] ReplaceSubmissionCommand command)
        {
            command = command with { SubmissionId = submissionId };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("{submissionId:int}")]
        [SwaggerOperation(
    Summary = "Delete submission",
    Description = "Deletes a submission."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubmission(int submissionId)
        {
            var command = new DeleteSubmissionCommand(submissionId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{submissionId:int}/grade")]
        [SwaggerOperation(
    Summary = "Grade submission",
    Description = "Grades a submission and optionally adds instructor feedback."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GradeSubmission(
    int submissionId,
    [FromBody] GradeSubmissionCommand command)
        {
            command = command with { SubmissionId = submissionId };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
