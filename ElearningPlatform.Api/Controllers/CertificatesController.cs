using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Certificates.Commands.GenerateCertificate;
using ElearningPlatform.Application.Features.Certificates.Commands.RevokeCertificate;
using ElearningPlatform.Application.Features.Certificates.Queries.DownloadCertificate;
using ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateDetails;
using ElearningPlatform.Application.Features.Certificates.Queries.GetMyCertificates;
using ElearningPlatform.Application.Features.Certificates.Queries.VerifyCertificate;
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

        [HttpGet("{id}")]
        [SwaggerOperation(
         Summary = "Get certificate details",
         Description = "Retrieves the details of a specific certificate for the authenticated user."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCertificateDetails(int id)
        {
            var result = await mediator.Send(new GetCertificateDetailsQuery(id));
            return result.ToActionResult();
        }
        [HttpGet("{id}/download")]
        [SwaggerOperation(
    Summary = "Download certificate",
    Description = "Downloads the certificate for the authenticated user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadCertificate(int id)
        {
            var result = await mediator.Send(new DownloadCertificateQuery(id));
            return result.ToActionResult();
        }
        [HttpGet("verify")]
        [SwaggerOperation(
    Summary = "Verify certificate",
    Description = "Verifies a certificate using its verification code."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyCertificate(
    [FromQuery] VerifyCertificateQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpPatch("{id}/revoke")]
        [SwaggerOperation(
    Summary = "Revoke certificate",
    Description = "Revokes a certificate and records the reason for revocation."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RevokeCertificate(
    int id,
    [FromBody] RevokeCertificateCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
