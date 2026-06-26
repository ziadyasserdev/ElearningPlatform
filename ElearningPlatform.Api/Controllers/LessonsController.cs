using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Lessons.Commands.BulkDeleteLessons;
using ElearningPlatform.Application.Features.Lessons.Commands.BulkRestoreLessons;
using ElearningPlatform.Application.Features.Lessons.Commands.BulkToggleLessonPreview;
using ElearningPlatform.Application.Features.Lessons.Commands.BulkTogglePublishLessons;
using ElearningPlatform.Application.Features.Lessons.Commands.CreateLesson;
using ElearningPlatform.Application.Features.Lessons.Commands.DeleteLesson;
using ElearningPlatform.Application.Features.Lessons.Commands.EditLesson;
using ElearningPlatform.Application.Features.Lessons.Commands.PublishLesson;
using ElearningPlatform.Application.Features.Lessons.Commands.ReorderLessons;
using ElearningPlatform.Application.Features.Lessons.Commands.RestoreLesson;
using ElearningPlatform.Application.Features.Lessons.Commands.ToggleLessonPreview;
using ElearningPlatform.Application.Features.Lessons.Commands.UnpublishLesson;
using ElearningPlatform.Application.Features.Lessons.Queries.GetInstructorLessonsBySection;
using ElearningPlatform.Application.Features.Lessons.Queries.GetLessonsBySection;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly IMediator mediator;

        public LessonsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("sections/{sectionId}/lessons")]
        [SwaggerOperation(
          Summary = "Create lesson",
          Description = "Creates a new lesson under a specific section"
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateLesson(
          [FromRoute] int sectionId,
          [FromBody] CreateLessonCommand command)
        {
            command.SectionId = sectionId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPut("lessons")]
        [SwaggerOperation(
    Summary = "Edit lesson",
    Description = "Update lesson title and description."
)]
        public async Task<IActionResult> EditLesson([FromBody] EditLessonCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("lessons/{id}")]
        [SwaggerOperation(
    Summary = "Delete lesson",
    Description = "Soft delete a lesson."
)]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var result = await mediator.Send(new DeleteLessonCommand { Id = id });
            return result.ToActionResult();
        }

        [HttpPatch("lessons/{id}/publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var result = await mediator.Send(new PublishLessonCommand { Id = id });
            return result.ToActionResult();
        }
        [HttpPatch("lessons/{id}/unpublish")]
        public async Task<IActionResult> Unpublish(int id)
        {
            var result = await mediator.Send(new UnpublishLessonCommand { Id = id });
            return result.ToActionResult();
        }
        [HttpGet("/sections/{sectionId}/lessons")]
        [SwaggerOperation(
        Summary = "Get lessons by section (student view)",
        Description = "Returns all published lessons inside a section ordered by OrderIndex"
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySection(int sectionId)
        {
            var result = await mediator.Send(
                new GetLessonsBySectionQuery
                {
                    SectionId = sectionId
                });

            return result.ToActionResult();
        }

        [HttpGet("/instructor/sections/{sectionId}/lessons")]
        [SwaggerOperation(
          Summary = "Get lessons by section (instructor view)",
          Description = "Returns all lessons including unpublished and deleted flags for course owner"
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBySectionForInstructor(int sectionId)
        {
            var result = await mediator.Send(
                new GetInstructorLessonsBySectionQuery
                {
                    SectionId = sectionId
                });

            return result.ToActionResult();
        }

        [HttpPatch("{id}/restore")]
        [SwaggerOperation(
    Summary = "Restore deleted lesson",
    Description = "Restores a soft-deleted lesson if no conflict exists"
)]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await mediator.Send(new RestoreLessonCommand
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpPut("sections/{sectionId}/lessons/reorder")]
        public async Task<IActionResult> Reorder(int sectionId, [FromBody] List<int> lessonIds)
        {
            var result = await mediator.Send(new ReorderLessonsCommand
            {
                SectionId = sectionId,
                LessonIds = lessonIds
            });

            return result.ToActionResult();
        }

        [HttpPatch("lessons/{id}/toggle-preview")]
        public async Task<IActionResult> TogglePreview(int id)
        {
            var result = await mediator.Send(new ToggleLessonPreviewCommand
            {
                LessonId = id
            });

            return result.ToActionResult();
        }
        [HttpPatch("lessons/bulk-preview")]
        public async Task<IActionResult> BulkPreview([FromBody] BulkToggleLessonPreviewCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("bulk")]
        [SwaggerOperation(
          Summary = "Bulk delete lessons",
          Description = "Soft deletes multiple lessons by IDs"
      )]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteLessonsCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

       
        [HttpPatch("bulk/restore")]
        [SwaggerOperation(
            Summary = "Bulk restore lessons",
            Description = "Restores multiple soft-deleted lessons"
        )]
        public async Task<IActionResult> BulkRestore([FromBody] BulkRestoreLessonsCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

       
        [HttpPatch("bulk/publish")]
        [SwaggerOperation(
            Summary = "Bulk publish/unpublish lessons",
            Description = "Change publish status for multiple lessons"
        )]
        public async Task<IActionResult> BulkPublish([FromBody] BulkTogglePublishLessonsCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

    }
}
