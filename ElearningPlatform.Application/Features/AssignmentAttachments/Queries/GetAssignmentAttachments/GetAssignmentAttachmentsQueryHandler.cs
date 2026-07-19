using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.AssignmentAttachments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Queries.GetAssignmentAttachments
{
    public class GetAssignmentAttachmentsQueryHandler
       : IRequestHandler<GetAssignmentAttachmentsQuery, Result<List<AssignmentAttachmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAssignmentAttachmentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<AssignmentAttachmentDto>>> Handle(
            GetAssignmentAttachmentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<AssignmentAttachmentDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .Include(a => a.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<List<AssignmentAttachmentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<List<AssignmentAttachmentDto>>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to view these attachments.");
            }

            var attachments = await unitOfWork.AssignmentAttachments
                .Query()
                .Where(a =>
                    a.AssignmentId == request.AssignmentId &&
                    !a.IsDeleted)
                .OrderBy(a => a.OrderIndex)
                .Select(a => new AssignmentAttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileUrl = a.FileUrl,
                    ContentType = a.ContentType,
                    FileSize = a.FileSize,
                    OrderIndex = a.OrderIndex
                })
                .ToListAsync(cancellationToken);

            return Result<List<AssignmentAttachmentDto>>.Success(attachments);
        }
    }
}



/*
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class AssignmentAttachment : BaseEntity
    {
        public int AssignmentId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public int OrderIndex { get; set; } = 1;

        public Assignment Assignment { get; set; } = null!;
    }
}

    public record AssignmentAttachmentDto
(
    int Id,
    string FileName,
    string FileUrl,
    string ContentType,
    long FileSize,
    int OrderIndex
);
}

 * 
 * 
 */
