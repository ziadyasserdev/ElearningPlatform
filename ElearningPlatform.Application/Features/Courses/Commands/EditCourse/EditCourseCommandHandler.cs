using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.EditCourse
{
    public class EditCourseCommandHandler : IRequestHandler<EditCourseCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public EditCourseCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(EditCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.Courses.Query()
                 .FirstOrDefaultAsync(x => x.Id == request.CourseId
                 && !x.IsDeleted && x.IsActive);
            if(course is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Course not found");

            var category = await unitOfWork.Categories.Query()
                .AnyAsync(x => x.Id == request.CategoryId
                && !x.IsDeleted && x.IsActive);
            if (!category)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found");

            var checkTitle = await unitOfWork.Courses.Query()
             .AnyAsync(x => x.Title == request.Title && x.InstructorId
             == course.InstructorId 
             && x.Id != request.CourseId
             && !x.IsDeleted);
            if (checkTitle)
                return Result<int>.Failure(ResultStatus.Conflict, "Course title already exists for this instructor");
            bool isChanged =
         course.Title != request.Title ||
         course.Description != request.Description ||
         course.ShortDescription != request.ShortDescription ||
         course.Price != request.Price ||
         course.DiscountPrice != request.DiscountPrice ||
         course.DiscountEndDate != request.DiscountEndDate ||
         course.Language != request.Language ||
         course.Level != (CourseLevel)request.Level ||
         course.CategoryId != request.CategoryId;

         
            if (!isChanged)
                return Result<int>.Success(course.Id);


            course.Title = request.Title;
            course.Description = request.Description;
            course.ShortDescription = request.ShortDescription;
            course.Price = request.Price;
            course.DiscountPrice = request.DiscountPrice;
            course.DiscountEndDate = request.DiscountEndDate;
            course.Language = request.Language;
            course.Level = (CourseLevel)request.Level;
            course.CategoryId = request.CategoryId;
            course.Slug = request.Title.ToLower().Replace(" ", "-");
            course.UpdatedAt = DateTime.UtcNow;
            course.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(course.Id);

        }
    }
}
