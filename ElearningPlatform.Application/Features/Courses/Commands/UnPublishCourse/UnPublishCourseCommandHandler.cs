using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.UnPublishCourse
{
    public class UnPublishCourseCommandHandler
         : IRequestHandler<UnPublishCourseCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UnPublishCourseCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UnPublishCourseCommand request,
            CancellationToken cancellationToken)
        {


            if(!currentUserService.IsAuthenticated)
        return Result<string>.Failure( ResultStatus.Unauthorized, "User is not authenticated");

            var userId = currentUserService.UserId;




            var course = await unitOfWork.Courses.Query()
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(
                    c => c.Id == request.CourseId &&
                         !c.IsDeleted,
                    cancellationToken);

            if (course is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");
            }

            if(course.Instructor.UserId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to unpublish this course");
            }



            if (course.Status != CourseStatus.Published)
          return Result<string>.Failure(
                ResultStatus.Conflict,
                "Course is not published");

            course.Status = CourseStatus.Draft;
            course.PublishedAt = null;

            course.UpdatedAt = DateTime.Now;
            course.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
               
                "Course unpublished successfully");
        }
    }
}
