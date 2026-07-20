using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Submissions.Commands.CreateSubmission;
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
    }
}
