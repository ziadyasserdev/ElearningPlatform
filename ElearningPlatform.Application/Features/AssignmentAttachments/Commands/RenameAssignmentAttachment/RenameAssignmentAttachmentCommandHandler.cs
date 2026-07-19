using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.RenameAssignmentAttachment
{
    public class RenameAssignmentAttachmentCommandHandler
      : IRequestHandler<RenameAssignmentAttachmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RenameAssignmentAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RenameAssignmentAttachmentCommand request,
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
                    "You are not authorized to rename this attachment.");
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

            var duplicateName = await unitOfWork.AssignmentAttachments
                .Query()
                .AnyAsync(a =>
                    a.AssignmentId == attachment.AssignmentId &&
                    a.Id != attachment.Id &&
                    !a.IsDeleted &&
                    a.FileName == request.FileName,
                    cancellationToken);

            if (duplicateName)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "An attachment with the same name already exists.");
            }

            if (attachment.FileName == request.FileName)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Attachment already has this name.");
            }

            attachment.FileName = request.FileName;
            attachment.UpdatedAt = DateTime.Now;
            attachment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Attachment renamed successfully.");
        }
    }
}
