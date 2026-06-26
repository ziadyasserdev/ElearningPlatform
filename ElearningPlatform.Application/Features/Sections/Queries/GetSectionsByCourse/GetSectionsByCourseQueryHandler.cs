using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Sections.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionsByCourse
{
    public class GetSectionsByCourseQueryHandler : IRequestHandler<GetSectionsByCourseQuery, Result<List<SectionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetSectionsByCourseQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<SectionDto>>> Handle(GetSectionsByCourseQuery request, CancellationToken cancellationToken)
        {
           var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(x => x.Id == request.CourseId 
                && !x.IsDeleted
                && x.IsActive, cancellationToken);
               
            if(course is null)
                return Result<List<SectionDto>>.Failure(ResultStatus.NotFound,"Course not found");
            var sections = await unitOfWork.Sections.Query()
                .Where(x => x.CourseId == request.CourseId
                && !x.IsDeleted)
                .OrderBy(x => x.OrderIndex)
                .Select(x => new SectionDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    OrderIndex = x.OrderIndex
                   

                }).ToListAsync(cancellationToken);

            if(!sections.Any())
                return Result<List<SectionDto>>.Failure(ResultStatus.NotFound,"No sections found for this course");
            return Result<List<SectionDto>>.Success(sections);
        }
    }
}
