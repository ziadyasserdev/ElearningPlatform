using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Exams.Commands.UpdateAnswer;
using ElearningPlatform.Application.Features.Questions.Commands.CreateAnswer;
using ElearningPlatform.Application.Features.Questions.Commands.CreateQuestion;
using ElearningPlatform.Application.Features.Questions.Commands.DeleteAnswer;
using ElearningPlatform.Application.Features.Questions.Commands.DeleteQuestion;
using ElearningPlatform.Application.Features.Questions.Commands.UpdateAnswer;
using ElearningPlatform.Application.Features.Questions.Commands.UpdateQuestion;
using ElearningPlatform.Application.Features.Questions.Dtos;
using ElearningPlatform.Application.Features.Questions.Queries.GetExamQuestions;
using ElearningPlatform.Application.Features.Questions.Queries.GetQuestionAnswers;
using ElearningPlatform.Application.Features.Questions.Queries.GetQuestionById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public QuestionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("{id}")]
        [SwaggerOperation(
    Summary = "Get question by id",
    Description = "Retrieves the details of a specific question."
)]
        [ProducesResponseType(typeof(Result<QuestionDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var result = await mediator.Send(new GetQuestionByIdQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpGet("exam/{examId}/questions")]
        [SwaggerOperation(
    Summary = "Get exam questions",
    Description = "Retrieves all questions for a specific exam."
)]
        [ProducesResponseType(typeof(Result<List<QuestionDetailsDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExamQuestions(int examId)
        {
            var result = await mediator.Send(new GetExamQuestionsQuery
            {
                ExamId = examId
            });

            return result.ToActionResult();
        }
        [HttpPost]
        [SwaggerOperation(
    Summary = "Create question",
    Description = "Creates a new question for a specific exam."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateQuestion([FromQuery] CreateQuestionCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPost("questions/{questionId}/answers")]
        [SwaggerOperation(
    Summary = "Create answer",
    Description = "Creates a new answer for a specific question."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAnswer(
    int questionId,
    CreateAnswerCommand command)
        {
            command.QuestionId = questionId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Update question",
    Description = "Updates an existing question."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateQuestion(
    int id,
   [FromQuery] UpdateQuestionCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("answers/{id}")]
        [SwaggerOperation(
    Summary = "Update answer",
    Description = "Updates an existing answer."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAnswer(
    int id,
   UpdateQuestionAnswerCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }



        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete question",
    Description = "Deletes an existing question."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result = await mediator.Send(new DeleteQuestionCommand
            {
                Id = id
            });

            return result.ToActionResult();
        }

        [HttpDelete("answers/{id}")]
        [SwaggerOperation(
    Summary = "Delete answer",
    Description = "Deletes an existing answer."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var result = await mediator.Send(new DeleteAnswerCommand
            {
                Id = id
            });

            return result.ToActionResult();
        }

        [HttpGet("questions/{questionId}/answers")]
        [SwaggerOperation(
    Summary = "Get question answers",
    Description = "Retrieves all answers for a specific question."
)]
        [ProducesResponseType(typeof(Result<List<AnswerDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestionAnswers(int questionId)
        {
            var result = await mediator.Send(new GetQuestionAnswersQuery
            {
                QuestionId = questionId
            });

            return result.ToActionResult();
        }
    }
}
