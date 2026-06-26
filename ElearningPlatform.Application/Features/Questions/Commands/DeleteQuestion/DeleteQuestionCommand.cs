using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.DeleteQuestion
{
    public class DeleteQuestionCommand
    : IRequest<Result<string>>
    {
        public int Id { get; set; }
    }
}
