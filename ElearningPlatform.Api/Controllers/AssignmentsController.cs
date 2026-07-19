using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Assignments.Commands.CloseAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.CreateAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.DeleteAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.ExtendAssignmentDeadline;
using ElearningPlatform.Application.Features.Assignments.Commands.PublishAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.ReopenAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.RestoreAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.UnPublishAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.UpdateAssignment;
using ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentById;
using ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentStatistics;
using ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentSubmissions;
using ElearningPlatform.Application.Features.Assignments.Queries.GetCourseAssignments;
using ElearningPlatform.Application.Features.Assignments.Queries.GetLateSubmissions;
using ElearningPlatform.Application.Features.Assignments.Queries.GetPendingStudents;
using ElearningPlatform.Application.Features.Assignments.Queries.GetTopStudents;
using ElearningPlatform.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AssignmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
    Summary = "Create a new assignment",
    Description = "Creates a new assignment for a specific course."
)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAssignment(
    [FromBody] CreateAssignmentCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get assignment by id",
    Description = "Retrieves the details of a specific assignment by its identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            var query = new GetAssignmentByIdQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("course/{courseId:int}")]
        [SwaggerOperation(
    Summary = "Get course assignments",
    Description = "Retrieves a paginated list of assignments for a specific course with optional search and publication status filters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseAssignments(
    int courseId,
    [FromQuery] string? search,
    [FromQuery] bool? isPublished,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = new GetCourseAssignmentsQuery(
                courseId,
                search,
                isPublished,
                pageNumber,
                pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpPut("{id:int}")]
        [SwaggerOperation(
    Summary = "Update assignment",
    Description = "Updates an existing assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAssignment(
    int id,
    [FromBody] UpdateAssignmentCommand command)
        {
            command = command with { Id = id };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
    Summary = "Delete assignment",
    Description = "Deletes an existing assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var command = new DeleteAssignmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/restore")]
        [SwaggerOperation(
    Summary = "Restore assignment",
    Description = "Restores a previously deleted assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreAssignment(int id)
        {
            var command = new RestoreAssignmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/publish")]
        [SwaggerOperation(
    Summary = "Publish assignment",
    Description = "Publishes an assignment, making it available to students."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PublishAssignment(int id)
        {
            var command = new PublishAssignmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/unpublish")]
        [SwaggerOperation(
    Summary = "Unpublish assignment",
    Description = "Unpublishes an assignment, making it unavailable to students."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnPublishAssignment(int id)
        {
            var command = new UnPublishAssignmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/close")]
        [SwaggerOperation(
    Summary = "Close assignment",
    Description = "Closes an assignment and prevents further submissions."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CloseAssignment(int id)
        {
            var command = new CloseAssignmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/reopen")]
        [SwaggerOperation(
    Summary = "Reopen assignment",
    Description = "Reopens a closed assignment and allows submissions again."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReopenAssignment(int id)
        {
            var command = new ReopenAssignmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/extend-deadline")]
        [SwaggerOperation(
    Summary = "Extend assignment deadline",
    Description = "Extends the due date of an existing assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExtendAssignmentDeadline(
    int id,
    [FromBody] ExtendAssignmentDeadlineCommand command)
        {
            command = command with { Id = id };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}/statistics")]
        [SwaggerOperation(
    Summary = "Get assignment statistics",
    Description = "Retrieves statistics for a specific assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignmentStatistics(int id)
        {
            var query = new GetAssignmentStatisticsQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("{assignmentId:int}/submissions")]
        [SwaggerOperation(
    Summary = "Get assignment submissions",
    Description = "Retrieves a paginated list of submissions for a specific assignment with optional search and status filters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignmentSubmissions(
    int assignmentId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] SubmissionStatus? status = null)
        {
            var query = new GetAssignmentSubmissionsQuery(
                assignmentId,
                pageNumber,
                pageSize,
                search,
                status);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("{assignmentId:int}/pending-students")]
        [SwaggerOperation(
    Summary = "Get pending students",
    Description = "Retrieves a paginated list of students who have not submitted the specified assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPendingStudents(
    int assignmentId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = new GetPendingStudentsQuery(
                assignmentId,
                pageNumber,
                pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("{assignmentId:int}/late-submissions")]
        [SwaggerOperation(
    Summary = "Get late submissions",
    Description = "Retrieves a paginated list of late submissions for a specific assignment with an optional search filter."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLateSubmissions(
    int assignmentId,
    [FromQuery] string? search,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = new GetLateSubmissionsQuery(
                assignmentId,
                search,
                pageNumber,
                pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpGet("{assignmentId:int}/top-students")]
        [SwaggerOperation(
    Summary = "Get top students",
    Description = "Retrieves the top students for a specific assignment based on their scores."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTopStudents(
    int assignmentId,
    [FromQuery] int count = 10)
        {
            var query = new GetAssignmentTopStudentsQuery(
                assignmentId,
                count);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
    }
}
