using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.AssignmentAttachments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Queries.DownloadAssignmentAttachment
{
    public class DownloadAssignmentAttachmentQueryHandler
      : IRequestHandler<DownloadAssignmentAttachmentQuery, Result<FileDownloadDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public DownloadAssignmentAttachmentQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<FileDownloadDto>> Handle(
            DownloadAssignmentAttachmentQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<FileDownloadDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var attachment = await unitOfWork.AssignmentAttachments
                .Query()
                .Include(a => a.Assignment)
                    .ThenInclude(a => a.Course)
                    .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(a =>
                    a.Id == request.Id &&
                    !a.IsDeleted,
                    cancellationToken);

            if (attachment is null)
            {
                return Result<FileDownloadDto>.Failure(
                    ResultStatus.NotFound,
                    "Attachment not found.");
            }

           
            if (attachment.Assignment.Course.Instructor.UserId != userId)
            {
                return Result<FileDownloadDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to download this attachment.");
            }

            var downloadResult = await fileService.DownloadFileAsync(attachment.FileUrl);

            if (!downloadResult.IsSuccess)
            {
                return Result<FileDownloadDto>.Failure(
                    downloadResult.Status,
                    downloadResult.Error);
            }

            var dto = new FileDownloadDto
            {
                FileBytes = downloadResult.Value,
                FileName = attachment.FileName,
                ContentType = attachment.ContentType
            };

            return Result<FileDownloadDto>.Success(dto);
        }
    }
}
