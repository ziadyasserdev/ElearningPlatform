using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.RejectReview
{
    public class RejectReviewCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public string RejectionReason { get; set; } = string.Empty;
    }
}
