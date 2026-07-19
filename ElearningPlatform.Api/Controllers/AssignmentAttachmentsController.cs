using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.AssignmentAttachments.Commands.DeleteAssignmentAttachment;
using ElearningPlatform.Application.Features.AssignmentAttachments.Commands.UploadAssignmentAttachment;
using ElearningPlatform.Application.Features.AssignmentAttachments.Queries.DownloadAssignmentAttachment;
using ElearningPlatform.Application.Features.AssignmentAttachments.Queries.GetAssignmentAttachments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentAttachmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AssignmentAttachmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("{assignmentId:int}/attachments")]
        [SwaggerOperation(
    Summary = "Upload assignment attachment",
    Description = "Uploads an attachment for a specific assignment."
)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadAssignmentAttachment(
    int assignmentId,
    [FromForm] UploadAssignmentAttachmentCommand command)
        {
            command = command with { AssignmentId = assignmentId };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("{assignmentId:int}/attachments")]
        [SwaggerOperation(
    Summary = "Get assignment attachments",
    Description = "Retrieves all attachments for a specific assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignmentAttachments(int assignmentId)
        {
            var query = new GetAssignmentAttachmentsQuery
            {
                AssignmentId = assignmentId
            };

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
        [HttpDelete("attachments/{id:int}")]
        [SwaggerOperation(
    Summary = "Delete assignment attachment",
    Description = "Deletes an attachment from an assignment."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAssignmentAttachment(int id)
        {
            var command = new DeleteAssignmentAttachmentCommand(id);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}/download")]
        [SwaggerOperation(
      Summary = "Download assignment attachment",
      Description = "Downloads a specific assignment attachment."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Download(int id)
        {
            var result = await mediator.Send(
                new DownloadAssignmentAttachmentQuery(id));
            if(!result.IsSuccess)
            {
                return result.ToActionResult();
            }


            return File(
                result.Value!.FileBytes,
                result.Value.ContentType,
                result.Value.FileName);
        }
    }
}
