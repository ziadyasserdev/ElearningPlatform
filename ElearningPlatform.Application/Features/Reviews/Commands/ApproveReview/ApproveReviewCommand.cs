using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.ApproveReview
{
    public class ApproveReviewCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public ApproveReviewCommand(int id)
        {
            Id = id;
        }
    }
}
