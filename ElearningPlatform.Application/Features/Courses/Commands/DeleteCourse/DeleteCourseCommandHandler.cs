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

namespace ElearningPlatform.Application.Features.Courses.Commands.DeleteCourse
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteCourseCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.Courses.Query()
                .Where(x => x.Id == request.CourseId && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
            if(course is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Course not found");
            if (course.Status == CourseStatus.Published && course.TotalStudents > 0)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Cannot delete published course with enrolled students");
            }
            course.IsDeleted = true;
            course.IsActive = false;
            course.DeletedAt = DateTime.Now;
            course.DeletedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
                return Result<int>.Success(course.Id);
        }
    }
}
