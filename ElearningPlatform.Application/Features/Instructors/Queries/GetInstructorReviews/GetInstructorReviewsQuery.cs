using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorReviews
{
    public class GetInstructorReviewsQuery:IRequest<Result<List<InstructorReviewDto>>>
    {
        public int InstructorId { get; set; }

        public GetInstructorReviewsQuery(int instructorId)
        {
            InstructorId = instructorId;
        }
    }
}
