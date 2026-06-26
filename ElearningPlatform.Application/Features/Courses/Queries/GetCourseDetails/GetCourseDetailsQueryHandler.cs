using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Courses.Dtos;
using ElearningPlatform.Application.Features.Lessons.Dtos;
using ElearningPlatform.Application.Features.Sections.Dtos;
using ElearningPlatform.Application.Features.Videos.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetCourseDetails
{
    public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, Result<CourseDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCourseDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
 
        public async Task<Result<CourseDetailsDto>> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.Courses.Query()
                .Where(c => c.Id == request.Id && !c.IsDeleted &&
                c.IsActive).Select(x => new CourseDetailsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    InstructorName = x.Instructor.User.FullName,
                    CategoryName = x.Category.Name,
                    DiscountPrice = x.DiscountPrice,
                    Price = x.Price,
                  
                    Sections = x.Sections.Where(s => !s.IsDeleted && s.IsActive)
                    .OrderBy(s => s.OrderIndex)
                    .Select(s => new SectionForCourseDto
                    {
                        Id = s.Id,
                        Title = s.Title,
                        OrderIndex = s.OrderIndex,
                        
                        Lessons = s.Lessons.Where(l => !l.IsDeleted && l.IsPublished )
                       
                        .OrderBy(l => l.OrderIndex)
                        .Select(l => new LessonDetailsDto
                        {
                            Id = l.Id,
                            Title = l.Title,
                            Duration = l.Duration,
                            IsPreview = l.IsPreview,
                            
                            Videos = l.Videos.Where(v => !v.IsDeleted).Select(v => new VideoDto
                            {
                                Id = v.Id,
                              Url = v.FileUrl,
                              
                            }).ToList()
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
            if(course == null)
                return Result<CourseDetailsDto>.Failure(ResultStatus.NotFound, "Course not found");
            return Result<CourseDetailsDto>.Success(course);

        }
    }
}
