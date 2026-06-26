using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Sections.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionsCountByCourse
{
    public class GetSectionsCountByCourseQueryHandler : IRequestHandler<GetSectionsCountByCourseQuery, Result<SectionsDashboardDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetSectionsCountByCourseQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<SectionsDashboardDto>> Handle(GetSectionsCountByCourseQuery request, CancellationToken cancellationToken)
        {
            var courseExist = await unitOfWork.Courses.Query()
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.Id && !x.IsDeleted 
                && x.IsActive);

            if (!courseExist)
                return Result<SectionsDashboardDto>.Failure(ResultStatus.NotFound, "Course not found");

            var totalSections = await unitOfWork.Sections.Query()
                .Where(c => c.CourseId == request.Id)
                 .CountAsync(cancellationToken);
            var activeSections = await unitOfWork.Sections.Query()
                .Where(c => c.CourseId == request.Id)
                .Where(x => x.IsActive && !x.IsDeleted)
                .CountAsync(cancellationToken);
            var inactiveSections = await unitOfWork.Sections.Query()
                .Where(c => c.CourseId == request.Id)
                .Where(x => !x.IsActive && !x.IsDeleted)
                .CountAsync(cancellationToken);
            var deletedSections = await unitOfWork.Sections.Query()
                .Where(c => c.CourseId == request.Id)
                .Where(x => x.IsDeleted)
                .CountAsync(cancellationToken);
            var sectionsDashboard = new SectionsDashboardDto
            {
                TotalSections = totalSections,
                ActiveSections = activeSections,
                InactiveSections = inactiveSections,
                DeletedSections = deletedSections
            };

            return Result<SectionsDashboardDto>.Success(sectionsDashboard);
        }
    }
}
