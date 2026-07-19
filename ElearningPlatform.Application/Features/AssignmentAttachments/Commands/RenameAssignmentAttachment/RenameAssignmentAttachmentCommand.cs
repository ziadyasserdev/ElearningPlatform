using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.RenameAssignmentAttachment
{
    public record RenameAssignmentAttachmentCommand(
     int Id,
     string FileName)
     : IRequest<Result<string>>;
}
