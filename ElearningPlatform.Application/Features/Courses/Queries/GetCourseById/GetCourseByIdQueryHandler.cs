using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Courses.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetCourseById
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Result<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCourseByIdQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<object>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
         
            if(currentUserService.IsInRole("Admin"))
            {
                var course = await unitOfWork.Courses.Query()
                   .Where(x => x.Id == request.Id)
                   .Select(x => new CourseAdminDto
                   {
                          Id = x.Id,
                          Title = x.Title,
                          Description = x.Description,
    
                          Slug = x.Slug,
    
                          IsDeleted = x.IsDeleted,
                          IsActive = x.IsActive,
                          CreatedAt = x.CreatedAt,
                          ShortDescription = x.ShortDescription,
                          Status = x.Status.ToString(),
                          TotalStudents = x.TotalStudents,
                          AverageRating = x.AverageRating,
                          CategoryName = x.Category.Name,
                          InstructorName = x.Instructor.User.FullName,
                          DiscountEndDate = x.DiscountEndDate,
                          DiscountPrice = x.DiscountPrice,
                          IsFeatured = x.IsFeatured,
                          PublishedAt = x.PublishedAt,
                          Price = x.Price,
                          TotalDurationInMinutes = x.TotalDurationInMinutes,
                          TotalLessons = x.TotalLessons,
                          TotalReviews = x.TotalReviews,
                          UpdatedAt = x.UpdatedAt

                   }).FirstOrDefaultAsync(cancellationToken);
                if(course == null)
                {
                    return Result<object>.Failure(ResultStatus.NotFound,"Course not found");
                }
                return Result<object>.Success(course, "Course retrieved successfully");
            }
            else
            {
                var course = await unitOfWork.Courses.Query()
                  .Where(x => x.Id == request.Id && !x.IsDeleted
                  && x.Status == Domain.Enums.CourseStatus.Published
                  && x.IsActive)
                  .Select(x => new CourseUserDto
                  {

                      Id = x.Id,
                      Title = x.Title,
                      
                      ShortDescription = x.ShortDescription,
                      TotalStudents = x.TotalStudents,
                      DiscountPrice = x.DiscountPrice,
                      AverageRating = x.AverageRating ,
                      CategoryName = x.Category.Name,
                      InstructorName = x.Instructor.User.FullName,
                      Language = x.Language,
                      Level = x.Level.ToString(),
                      Price = x.Price,
                      ThumbnailUrl = x.ThumbnailUrl!

                  }).FirstOrDefaultAsync(cancellationToken);

                if(course == null)
                {
                    return Result<object>.Failure(ResultStatus.NotFound, "Course not found");
                }

                return Result<object>.Success(course, "Course retrieved successfully");
            }
        }
    }
}
