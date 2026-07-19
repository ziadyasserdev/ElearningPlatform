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

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.ReplaceAssignmentAttachment
{
    public class ReplaceAssignmentAttachmentCommandHandler
      : IRequestHandler<ReplaceAssignmentAttachmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public ReplaceAssignmentAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<string>> Handle(
            ReplaceAssignmentAttachmentCommand request,
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
                    "You are not authorized to replace this attachment.");
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

            var duplicateFile = await unitOfWork.AssignmentAttachments
                .Query()
                .AnyAsync(a =>
                    a.AssignmentId == attachment.AssignmentId &&
                    a.Id != attachment.Id &&
                    !a.IsDeleted &&
                    a.FileName == request.File.FileName,
                    cancellationToken);

            if (duplicateFile)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "A file with the same name already exists.");
            }

            var uploadResult = await fileService.UploadFileAsync(request.File);

            if (!uploadResult.IsSuccess)
            {
                return Result<string>.Failure(
                    uploadResult.Status,
                    uploadResult.Error);
            }

            var oldFileUrl = attachment.FileUrl;

            try
            {
                attachment.FileName = request.File.FileName;
                attachment.FileUrl = uploadResult.Value;
                attachment.ContentType = request.File.ContentType;
                attachment.FileSize = request.File.Length;

                attachment.UpdatedAt = DateTime.UtcNow;
                attachment.UpdatedBy = currentUserService.UserName;

                await unitOfWork.SaveAsync();

                var removeResult = fileService.Remove(oldFileUrl);

             

                return Result<string>.Success("Attachment replaced successfully.");
            }
            catch
            {
             
                fileService.Remove(uploadResult.Value);

                throw;
            }
        }
    }
}
