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

namespace ElearningPlatform.Application.Features.Courses.Commands.UploadCourseThumbnail
{
    public class UploadCourseThumbnailCommandHandler : IRequestHandler<UploadCourseThumbnailCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public UploadCourseThumbnailCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }
        public async Task<Result<string>> Handle(UploadCourseThumbnailCommand request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var userId = currentUserService.UserId;
           var course = await unitOfWork.Courses.Query()
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(x => x.Id == request.CourseId 
                && !x.IsDeleted 
               
                && x.IsActive
                , cancellationToken);
            if(course is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Course not found");
            if(course.Instructor.UserId != userId)
                return Result<string>.Failure(ResultStatus.Forbidden, "User is not authorized to upload thumbnail for this course");
            if(course.ThumbnailUrl is not null)
                return Result<string>.Failure(ResultStatus.Conflict, "Course already has a thumbnail");
            var uploadResult = await fileService.UploadImageAsync(request.File);
            if(!uploadResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure, uploadResult.Error);
            course.ThumbnailUrl = uploadResult.Value;
            course.UpdatedAt = DateTime.Now;
            course.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<string>.Success( "Thumbnail uploaded successfully");

        }
    }
}
