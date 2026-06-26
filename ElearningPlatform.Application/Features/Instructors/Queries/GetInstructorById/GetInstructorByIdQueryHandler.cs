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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorById
{
    public class GetInstructorByIdQueryHandler : IRequestHandler<GetInstructorByIdQuery, Result<InstructorDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetInstructorByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<InstructorDto>> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            var instructor=await unitOfWork.Instructors.Query()
                .Where(x => x.Id == request.Id && !x.User.IsDeleted && x.User.IsInstructor && x.User.IsActive)
                .Select(x => new InstructorDto
                {
                    Id = x.Id,
                    FullName = x.User.FullName,
                    ProfileImageUrl = x.User.ProfileImageUrl,
                    Bio = x.Bio,
                    ExperienceYears = x.ExperienceYears,
                    Rating = x.Rating,
                    Specialization = x.Specialization,
                }).FirstOrDefaultAsync(cancellationToken);
            if(instructor == null)
                return Result<InstructorDto>.Failure(ResultStatus.NotFound, "Instructor not found");
           return Result<InstructorDto>.Success(instructor);
        }
    }
}
