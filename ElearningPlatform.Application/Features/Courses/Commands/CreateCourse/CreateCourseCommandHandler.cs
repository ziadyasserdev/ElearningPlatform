using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateCourseCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
                .AnyAsync(x => x.Id == request.CategoryId
                && !x.IsDeleted && x.IsActive);
            if (!category)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found");
            var instructor = await unitOfWork.Instructors.Query()
                .FirstOrDefaultAsync(x => x.Id == request.InstructorId
                && !x.User.IsDeleted && x.User.IsActive && x.User.IsInstructor);
            if(instructor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Instructor not found");
            var checkTitle = await unitOfWork.Courses.Query()
                .AnyAsync(x => x.Title == request.Title && x.InstructorId
                == request.InstructorId && !x.IsDeleted);
            if(checkTitle)
                return Result<int>.Failure(ResultStatus.Conflict, "Course title already exists for this instructor");
            var slug = Regex.Replace(request.Title.ToLower(), @"[^a-z0-9\s-]", "")
      .Replace(" ", "-");


            var slugExists = await unitOfWork.Courses.Query()
                .AnyAsync(c => c.Slug == slug, cancellationToken);

            if (slugExists)
                slug += "-" + Guid.NewGuid().ToString().Substring(0, 6);
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                ShortDescription = request.ShortDescription,
                Slug = slug,
                Price = request.Price,
                DiscountPrice = request.DiscountPrice,
                DiscountEndDate = request.DiscountEndDate,
                CategoryId = request.CategoryId,
                InstructorId = request.InstructorId,
                Language = request.Language,
                Level =(CourseLevel)request.Level,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
                Status = CourseStatus.Draft,
                
            };
    
            instructor.TotalCourses++;

           
            await unitOfWork.Courses.AddAsync(course);
      
        
            await unitOfWork.SaveAsync();
            return Result<int>.Success(course.Id);
        }
    }
}
