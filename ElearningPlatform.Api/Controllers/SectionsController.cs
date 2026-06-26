using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Sections.Commands.ActivateSection;
using ElearningPlatform.Application.Features.Sections.Commands.BulkDeleteSection;
using ElearningPlatform.Application.Features.Sections.Commands.BulkRestoreSection;
using ElearningPlatform.Application.Features.Sections.Commands.BulkUpdateSectionStatus;
using ElearningPlatform.Application.Features.Sections.Commands.CreateSection;
using ElearningPlatform.Application.Features.Sections.Commands.DeactivateSection;
using ElearningPlatform.Application.Features.Sections.Commands.DeleteSection;
using ElearningPlatform.Application.Features.Sections.Commands.DuplicateSection;
using ElearningPlatform.Application.Features.Sections.Commands.EditSection;
using ElearningPlatform.Application.Features.Sections.Commands.ReorderSection;
using ElearningPlatform.Application.Features.Sections.Commands.RestoreSection;
using ElearningPlatform.Application.Features.Sections.Queries.GetSectionById;
using ElearningPlatform.Application.Features.Sections.Queries.GetSectionsByCourse;
using ElearningPlatform.Application.Features.Sections.Queries.GetSectionsCountByCourse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SectionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("courses/{courseId}/sections")]
        [SwaggerOperation(
    Summary = "Get sections by course",
    Description = "Retrieves all sections belonging to a specific course by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSectionsByCourse([FromRoute] int courseId)
        {
            var result = await mediator.Send(new GetSectionsByCourseQuery(courseId));
            return result.ToActionResult();
        }



        [HttpGet("sections/{id}")]
        [SwaggerOperation(
            Summary = "Get section by id",
            Description = "Retrieves a specific section by its unique identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await mediator.Send(new GetSectionByIdQuery(id));
            return result.ToActionResult();
        }
        [HttpGet("courses/{courseId}/sections/count")]
        [SwaggerOperation(
    Summary = "Get sections count by course",
    Description = "Returns the number of active sections for a specific course."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSectionsCount([FromRoute] int courseId)
        {
            var result = await mediator.Send(new GetSectionsCountByCourseQuery(courseId));
            return result.ToActionResult();
        }

        [HttpPost("courses/{courseId}/sections")]
        [SwaggerOperation(
    Summary = "Create section for a course",
    Description = "Creates a new section under a specific course باستخدام CourseId."
)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(
    [FromRoute] int courseId,
    [FromBody] CreateSectionCommand command)
        {
            command.CourseId = courseId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPut("sections/{id}")]
        [SwaggerOperation(
    Summary = "Update section",
    Description = "Updates an existing section by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
    [FromRoute] int id,
    [FromBody] EditSectionCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("sections/{id}")]
        [SwaggerOperation(
     Summary = "Delete section",
     Description = "Deletes an existing section by its unique identifier."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSection([FromRoute] int id)
        {
            var result = await mediator.Send(new DeleteSectionCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("sections/{id}/restore")]
        [SwaggerOperation(
    Summary = "Restore section",
    Description = "Restores a previously deleted section by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore([FromRoute] int id)
        {
            var result = await mediator.Send(new RestoreSectionCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("sections/{id}/activate")]
        [SwaggerOperation(
    Summary = "Activate section",
    Description = "Activates a section by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Activate([FromRoute] int id)
        {
            var result = await mediator.Send(new ActivateSectionCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("sections/{id}/deactivate")]
        [SwaggerOperation(
    Summary = "Deactivate section",
    Description = "Deactivates a section by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            var result = await mediator.Send(new DeactivateSectionCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("sections/{id}/reorder")]
        [SwaggerOperation(
    Summary = "Reorder section",
    Description = "Updates the order of a section by specifying a new position."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reorder([FromRoute] int id, [FromQuery] int newOrder)
        {
            var command = new ReorderSectionCommand
            {
                SectionId = id,
                NewOrder = newOrder
            };

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("sections/{id}/duplicate")]
        [SwaggerOperation(
    Summary = "Duplicate section",
    Description = "Creates a copy of an existing section by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Duplicate([FromRoute] int id)
        {
            var result = await mediator.Send(new DuplicateSectionCommand(id));
            return result.ToActionResult();
        }

        [HttpPatch("sections/bulk-status")]
        [SwaggerOperation(
    Summary = "Bulk update section status",
    Description = "Activates or deactivates multiple sections by providing a list of section IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] BulkUpdateSectionStatusCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("sections/bulk")]
        [SwaggerOperation(
    Summary = "Bulk delete sections",
    Description = "Soft deletes multiple sections by providing a list of section IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteSectionCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("sections/bulk-restore")]
        [SwaggerOperation(
    Summary = "Bulk restore sections",
    Description = "Restores multiple soft-deleted sections by IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkRestore([FromBody] BulkRestoreSectionCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
