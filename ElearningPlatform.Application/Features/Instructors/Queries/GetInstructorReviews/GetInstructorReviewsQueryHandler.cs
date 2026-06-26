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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorReviews
{
    public class GetInstructorReviewsQueryHandler : IRequestHandler<GetInstructorReviewsQuery, Result<List<InstructorReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetInstructorReviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<InstructorReviewDto>>> Handle(GetInstructorReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Reviews.Query()
                .AsNoTracking()
                 .Where(x => x.Course.InstructorId == request.InstructorId)
                 .Select(x => new InstructorReviewDto
                 {
                     StudentName = x.Student.FullName,
                     Comment = x.Comment,
                     CourseTitle = x.Course.Title,
                     CreatedAt = x.CreatedAt,
                     Rating = x.Rating,

                 }).OrderByDescending(x => x.CreatedAt)
                 .ToListAsync(cancellationToken);
            return Result<List<InstructorReviewDto>>.Success(reviews);
        }
    }
}
