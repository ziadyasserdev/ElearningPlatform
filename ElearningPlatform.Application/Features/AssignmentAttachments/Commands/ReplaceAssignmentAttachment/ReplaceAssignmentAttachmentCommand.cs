using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.ReplaceAssignmentAttachment
{
    public record ReplaceAssignmentAttachmentCommand(
      int Id,
      IFormFile File)
      : IRequest<Result<string>>;
}
