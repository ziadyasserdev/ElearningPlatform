using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Assignments.Commands.CloseAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.CreateAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.DeleteAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.PublishAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.RestoreAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.UnPublishAssignment;
using ElearningPlatform.Application.Features.Assignments.Commands.UpdateAssignment;
using ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentById;
using ElearningPlatform.Application.Features.Assignments.Queries.GetCourseAssignments;
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
    }
}
