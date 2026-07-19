using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.AssignmentAttachments.Commands.UploadAssignmentAttachment
{

    public class UploadAssignmentAttachmentCommandHandler
        : IRequestHandler<UploadAssignmentAttachmentCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public UploadAssignmentAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<int>> Handle(
            UploadAssignmentAttachmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<int>.Failure(
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
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to upload attachments for this assignment.");
            }



         
            var hasSubmissions = await unitOfWork.Submissions
                .Query()
                .AnyAsync(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted,
                    cancellationToken);

            if (hasSubmissions)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Attachments cannot be modified after students have submitted.");
            }

          
            var attachmentsCount = await unitOfWork.AssignmentAttachments
                .Query()
                .CountAsync(a =>
                    a.AssignmentId == assignment.Id &&
                    !a.IsDeleted,
                    cancellationToken);

            if (attachmentsCount >= 10)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Maximum number of attachments reached.");
            }

         
            var exists = await unitOfWork.AssignmentAttachments
                .Query()
                .AnyAsync(a =>
                    a.AssignmentId == assignment.Id &&
                    a.FileName == request.File.FileName &&
                    !a.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A file with the same name already exists.");
            }



            var uploadResult = await fileService.UploadFileAsync(request.File);

            if (!uploadResult.IsSuccess)
            {
                return Result<int>.Failure(
                    uploadResult.Status,
                    uploadResult.Error);
            }

            try
            {
                var orderIndex = await unitOfWork.AssignmentAttachments
                    .Query()
                    .Where(a => a.AssignmentId == assignment.Id)
                    .CountAsync(cancellationToken) + 1;

                var attachment = new AssignmentAttachment
                {
                    AssignmentId = assignment.Id,
                    FileName = request.File.FileName,
                    FileUrl = uploadResult.Value!,
                    ContentType = request.File.ContentType,
                    FileSize = request.File.Length,
                    OrderIndex = orderIndex,
                    CreatedAt = DateTime.Now,
                    CreatedBy = currentUserService.UserName
                };

                await unitOfWork.AssignmentAttachments.AddAsync(attachment);

                await unitOfWork.SaveAsync();

                return Result<int>.Success(attachment.Id);
            }
            catch
            {
                fileService.Remove(uploadResult.Value);

                throw;
            }
        }
    }
}

