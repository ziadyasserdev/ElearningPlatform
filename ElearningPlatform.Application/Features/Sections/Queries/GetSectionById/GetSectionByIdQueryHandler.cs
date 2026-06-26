using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Lessons.Dtos;
using ElearningPlatform.Application.Features.Sections.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionById
{
    public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, Result<SectionDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetSectionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<SectionDetailsDto>> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
        {
           var section = await unitOfWork.Sections.Query()
              .Where(x => x.Id == request.Id && !x.IsDeleted)
              .Select(x => new SectionDetailsDto
              {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    OrderIndex = x.OrderIndex,
                    CourseId = x.CourseId,
                    Lessons = x.Lessons
                    .Where(l => !l.IsDeleted).Select(l => new LessonDto
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Description = l.Description,
                        OrderIndex = l.OrderIndex,
                       
                    }).ToList()

              }).FirstOrDefaultAsync(cancellationToken);

            if(section is null)
                return Result<SectionDetailsDto>.Failure(ResultStatus.NotFound,"Section not found");
            return Result<SectionDetailsDto>.Success(section);

        }
    }
}
