using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetAllInstructorsWithPagination
{
    public class GetAllInstructorsWithPaginationQueryHandler : IRequestHandler<GetAllInstructorsWithPaginationQuery, Result<PaginatedResult<InstructorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllInstructorsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<InstructorDto>>> Handle(GetAllInstructorsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Instructors.Query()
                 .AsNoTracking();

              var instructors=await query.Where(x => x.User.IsInstructor && !x.User.IsDeleted && x.User.IsActive)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new InstructorDto
                {
                    Id = x.Id,
                    FullName = x.User.FullName,
                    ProfileImageUrl = x.User.ProfileImageUrl,
                    Bio = x.Bio,
                    ExperienceYears = x.ExperienceYears,
                    Rating = x.Rating,
                    Specialization = x.Specialization,
                })
                .ToListAsync(cancellationToken);

            var TotalCount =await query.CountAsync(cancellationToken);
            var paginatedResult = new PaginatedResult<InstructorDto>(instructors,request.PageNumber,request.PageSize,TotalCount);
            return Result<PaginatedResult<InstructorDto>>.Success(paginatedResult);
        }
    }
}
