using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<Result<int>>
    {
        public int CourseId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}
