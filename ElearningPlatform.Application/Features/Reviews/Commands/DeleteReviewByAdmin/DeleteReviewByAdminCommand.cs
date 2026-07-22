using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.DeleteReviewByAdmin
{
    public class DeleteReviewByAdminCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public DeleteReviewByAdminCommand(int id)
        {
            Id = id;
        }
    }
}
