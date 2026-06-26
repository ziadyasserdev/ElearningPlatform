using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Courses.Commands.UpdateCourseThumbnail;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.DeleteCourseThumbnail
{
    public class DeleteCourseThumbnailCommandHandler : IRequestHandler<DeleteCourseThumbnailCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public DeleteCourseThumbnailCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }
        public async Task<Result<string>> Handle(DeleteCourseThumbnailCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var userId = currentUserService.UserId;
            var course = await unitOfWork.Courses.Query()
                 .Include(c => c.Instructor)
                 .FirstOrDefaultAsync(x => x.Id == request.CourseId
                 && !x.IsDeleted

                 && x.IsActive
                 , cancellationToken);
            if (course is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Course not found");
            if (course.Instructor.UserId != userId)
                return Result<string>.Failure(ResultStatus.Forbidden, "User is not authorized to upload thumbnail for this course");
            if (course.ThumbnailUrl is null)
             return Result<string>.Failure(ResultStatus.NotFound, "Course does not have a thumbnail to delete");

            if (!string.IsNullOrEmpty(course.ThumbnailUrl))
            {
                var deleteResult = fileService.Remove(course.ThumbnailUrl);
                if (!deleteResult.IsSuccess)
                    return Result<string>.Failure(ResultStatus.Failure, "Failed to delete existing thumbnail: " + deleteResult.Error);
            }
           
            course.ThumbnailUrl = null;
            course.UpdatedAt = DateTime.Now;
            course.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Thumbnail deleted successfully");

        }
    }
}
