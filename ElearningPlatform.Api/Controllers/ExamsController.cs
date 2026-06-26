using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Exams.Commands.CreateExam;
using ElearningPlatform.Application.Features.Exams.Commands.DeleteExam;
using ElearningPlatform.Application.Features.Exams.Commands.FinishExam;
using ElearningPlatform.Application.Features.Exams.Commands.StartExam;
using ElearningPlatform.Application.Features.Exams.Commands.SubmitAnswer;
using ElearningPlatform.Application.Features.Exams.Commands.UpdateAnswer;
using ElearningPlatform.Application.Features.Exams.Commands.UpdateExam;
using ElearningPlatform.Application.Features.Exams.Dtos;
using ElearningPlatform.Application.Features.Exams.Queriess.GetActiveAttempt;

//using ElearningPlatform.Application.Features.Exams.Queries.GetCourseExams;
using ElearningPlatform.Application.Features.Exams.Queriess.GetCourseExams;
using ElearningPlatform.Application.Features.Exams.Queriess.GetExamAttempts;
using ElearningPlatform.Application.Features.Exams.Queriess.GetExamById;
using ElearningPlatform.Application.Features.Exams.Queriess.GetExamDetailsForInstructor;
using ElearningPlatform.Application.Features.Exams.Queriess.GetExamDetailsForStudent;
using ElearningPlatform.Application.Features.Exams.Queriess.GetExamStatistics;
using ElearningPlatform.Application.Features.Exams.Queriess.GetFailedStudents;
using ElearningPlatform.Application.Features.Exams.Queriess.GetHardQuestions;
using ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResult;
using ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResults;
using ElearningPlatform.Application.Features.Exams.Queriess.GetTopStudents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ExamsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
    Summary = "Create exam",
    Description = "Creates a new exam for a specific course."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
           Summary = "Update exam",
           Description = "Updates exam details."
       )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] UpdateExamCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
          Summary = "Delete exam",
          Description = "Deletes an exam by its id."
      )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var result = await mediator.Send(new DeleteExamCommand
            {
                Id = id
            });

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
          Summary = "Get exam details for instructor",
          Description = "Retrieves full exam details including questions and structure for instructor view."
      )]
        [ProducesResponseType(typeof(Result<ExamInstructorDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExamDetails(int id)
        {
            var result = await mediator.Send(new GetExamDetailsForInstructorQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpGet("attempts/{attemptId}/result")]
        [SwaggerOperation(
    Summary = "Get my exam result",
    Description = "Retrieves the result of a completed exam attempt for the current student."
)]
        [ProducesResponseType(typeof(Result<MyExamResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyExamResult(int attemptId)
        {
            var result = await mediator.Send(new GetMyExamResultQuery
            {
                AttemptId = attemptId
            });

            return result.ToActionResult();
        }



        [HttpGet("my-results")]
        [SwaggerOperation(
    Summary = "Get my exam results",
    Description = "Retrieves all exam results for the current student."
)]
        [ProducesResponseType(typeof(Result<List<MyExamResultsDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyExamResults()
        {
            var result = await mediator.Send(new GetMyExamResultsQuery());

            return result.ToActionResult();
        }




        [HttpGet("course/{courseId}")]
        [SwaggerOperation(
    Summary = "Get course exams",
    Description = "Retrieves all exams associated with a specific course."
)]
        [ProducesResponseType(typeof(Result<List<ExamDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourseExams(int courseId)
        {
            var result = await mediator.Send(new GetCourseExammsQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }



        [HttpGet("{id}/statistics")]
        [SwaggerOperation(
         Summary = "Get exam statistics",
         Description = "Retrieves statistics for a specific exam including attempts, average score, and pass rate."
     )]
        [ProducesResponseType(typeof(Result<ExamStatisticsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExamStatistics(int id)
        {
            var result = await mediator.Send(new GetExamStatisticsQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpGet("{id}/attempts")]
        [SwaggerOperation(
          Summary = "Get exam attempts",
          Description = "Retrieves all student attempts for a specific exam."
      )]
        [ProducesResponseType(typeof(Result<List<ExamAttemptDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExamAttempts(int id)
        {
            var result = await mediator.Send(new GetExamAttemptsQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpGet("exam/{id}/top-students")]
        [SwaggerOperation(
       Summary = "Get top students",
       Description = "Retrieves top performing students in a specific exam."
   )]
        [ProducesResponseType(typeof(Result<List<TopStudentDtoo>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopStudents(int id)
        {
            var result = await mediator.Send(new GetTopStudentssQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpGet("exam/{id}")]
        [SwaggerOperation(
    Summary = "Get exam details for student",
    Description = "Retrieves exam details for a student before starting the exam."
)]
        [ProducesResponseType(typeof(Result<ExamDetailsForStudentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExamDetailsForStudent(int id)
        {
            var result = await mediator.Send(new GetExamDetailsForStudentQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }

        [HttpPost("exam/{id}/start")]
        [SwaggerOperation(
    Summary = "Start exam",
    Description = "Starts an exam attempt for the current student."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StartExam(int id)
        {
            var result = await mediator.Send(new StartExamCommand
            {
                ExamId = id
            });

            return result.ToActionResult();
        }
        [HttpPost("attempts/{attemptId}/answers")]
        [SwaggerOperation(
    Summary = "Submit answer",
    Description = "Submits or updates an answer for a question in an active exam attempt."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitAnswer(
    int attemptId,
    [FromBody] SubmitAnswerCommand command)
        {
            command.AttemptId = attemptId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("attempts/{attemptId}/answers")]
        [SwaggerOperation(
    Summary = "Update answer",
    Description = "Updates an existing answer in an active exam attempt."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAnswer(
    int attemptId,
    [FromBody] UpdateAnswerCommand command)
        {
            command.AttemptId = attemptId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost("attempts/{attemptId}/finish")]
        [SwaggerOperation(
    Summary = "Finish exam",
    Description = "Finishes the exam attempt and submits all answers for grading."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FinishExam(int attemptId)
        {
            var result = await mediator.Send(new FinishExamCommand
            {
                AttemptId = attemptId
            });

            return result.ToActionResult();
        }

        [HttpGet("{id}/failed-students")]
        [SwaggerOperation(
            Summary = "Get failed students",
            Description = "Retrieves students who failed a specific exam."
        )]
        [ProducesResponseType(typeof(Result<List<FailedStudentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFailedStudents(int id)
        {
            var result = await mediator.Send(new GetFailedStudentsQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }

        [HttpGet("exam/{id}/active-attempt")]
        [SwaggerOperation(
    Summary = "Get active exam attempt",
    Description = "Retrieves the current active attempt for the logged-in student."
)]
        [ProducesResponseType(typeof(Result<ActiveAttemptDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveAttempt(int id)
        {
            var result = await mediator.Send(new GetActiveAttemptQuery
            {
                ExamId = id
            });

            return result.ToActionResult();
        }


        [HttpGet("{id}/hard-questions")]
        [SwaggerOperation(
           Summary = "Get hard questions",
           Description = "Retrieves the most difficult questions in an exam based on incorrect answer rate."
       )]
        [ProducesResponseType(typeof(Result<List<HardQuestionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHardQuestions(int id)
        {
            var result = await mediator.Send(new GetHardQuestionsQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
       

        [HttpGet("by/{id}")]
        [SwaggerOperation(
         Summary = "Get exam by id",
         Description = "Retrieves exam details by exam id."
     )]
        [ProducesResponseType(typeof(Result<ExamDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExamById(int id)
        {
            var result = await mediator.Send(new GetExamByIdQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
    }
}
