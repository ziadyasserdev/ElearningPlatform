using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Certificates.Commands.GenerateCertificate;
using ElearningPlatform.Application.Features.Certificates.Queries.GetMyCertificates;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CertificatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("{courseId}/generate")]
        [SwaggerOperation(
    Summary = "Generate certificate",
    Description = "Generates a certificate for the authenticated user after successfully completing the course."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateCertificate(int courseId)
        {
            var result = await mediator.Send(new GenerateCertificateCommand(courseId));
            return result.ToActionResult();
        }
        [HttpGet("my-certificates")]
        [SwaggerOperation(
    Summary = "Get my certificates",
    Description = "Retrieves all certificates for the authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyCertificates()
        {
            var result = await mediator.Send(new GetMyCertificatesQuery());
            return result.ToActionResult();
        }
    }
}
