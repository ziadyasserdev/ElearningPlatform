using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetHardQuestions
{
    public class GetHardQuestionsQueryHandler
      : IRequestHandler<GetHardQuestionsQuery, Result<List<HardQuestionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetHardQuestionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<HardQuestionDto>>> Handle(
            GetHardQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<HardQuestionDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var result = await unitOfWork.Questions.Query()
                .Where(q =>
                    q.ExamId == request.Id &&
                    q.Exam.Course.Instructor.UserId == userId)
                .Select(q => new HardQuestionDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    AverageScore = q.StudentAnswers
    .Select(a => (double?)a.ScoreAwarded)
    .Average() ?? 0
                })
                .OrderBy(x => x.AverageScore)
                .ToListAsync(cancellationToken);

            return Result<List<HardQuestionDto>>.Success(result);
        }
    }
}
