using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.DeleteAssignmentAttachment
{
    public class DeleteAssignmentAttachmentCommandHandler
       : IRequestHandler<DeleteAssignmentAttachmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public DeleteAssignmentAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<string>> Handle(
            DeleteAssignmentAttachmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
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
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Attachment not found.");
            }

            if (attachment.Assignment.Course.Instructor.UserId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to delete this attachment.");
            }

            var hasSubmissions = await unitOfWork.Submissions
                .Query()
                .AnyAsync(s =>
                    s.AssignmentId == attachment.AssignmentId &&
                    !s.IsDeleted,
                    cancellationToken);

            if (hasSubmissions)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Attachments cannot be modified after students have submitted.");
            }

            var removeResult = fileService.Remove(attachment.FileUrl);

            if (!removeResult.IsSuccess)
            {
                return Result<string>.Failure(
                    removeResult.Status,
                    removeResult.Error);
            }

            attachment.IsDeleted = true;
            attachment.DeletedAt = DateTime.Now;
            attachment.DeletedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Attachment deleted successfully.");
        }
    }
}

