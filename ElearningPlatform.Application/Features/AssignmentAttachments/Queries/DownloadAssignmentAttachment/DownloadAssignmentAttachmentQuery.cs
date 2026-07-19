using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.AssignmentAttachments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Queries.DownloadAssignmentAttachment
{
    public record DownloadAssignmentAttachmentQuery(int Id)
        : IRequest<Result<FileDownloadDto>>;
}
