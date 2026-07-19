using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.AssignmentAttachments.Commands.UploadAssignmentAttachment;
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
    }
}
