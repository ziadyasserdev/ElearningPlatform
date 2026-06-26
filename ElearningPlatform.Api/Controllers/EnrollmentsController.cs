using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Enrollments.Commands.CancelEnrollment;
using ElearningPlatform.Application.Features.Enrollments.Commands.EnrollInCourse;
using ElearningPlatform.Application.Features.Enrollments.Commands.MarkCourseAsCompleted;

using ElearningPlatform.Application.Features.Enrollments.Dtos;
using ElearningPlatform.Application.Features.Enrollments.Queries.AdminGetAllEnrollments;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetCompletedCourses;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetContinueWatching;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetCourseProgress;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentById;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentDetails;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetMyEnrolled;
using ElearningPlatform.Application.Features.Enrollments.Queries.GetRecentCourses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public EnrollmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [SwaggerOperation(
      Summary = "Get all enrollments",
      Description = "Retrieves all enrollments with filtering, searching and pagination."
  )]
        [ProducesResponseType(typeof(Result<List<AdminEnrollmentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEnrollments(
      [FromQuery] GetAllEnrollmentsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
    Summary = "Get enrollment by id",
    Description = "Retrieves enrollment details by enrollment id."
)]
        [ProducesResponseType(typeof(Result<AdminEnrollmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEnrollmentById(int id)
        {
            var result = await mediator.Send(new GetEnrollmentByIdQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpPost("enroll")]
        [SwaggerOperation(
      Summary = "Enroll in course",
      Description = "Enroll the current user into a specific course."
  )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Enroll([FromBody] EnrollInCourseCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

     
        [HttpGet("my-enrolled-courses")]
        [SwaggerOperation(
    Summary = "Get my enrolled courses",
    Description = "Retrieves all courses that the currently authenticated student is enrolled in."
)]
        [ProducesResponseType(typeof(Result<List<MyEnrolledCourseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyEnrolledCourses()
        {
            var result = await mediator.Send(new GetMyEnrolledCoursesQuery());
            return result.ToActionResult();
        }
        [HttpGet("details/{courseId}")]
        [SwaggerOperation(
    Summary = "Get enrollment details",
    Description = "Retrieves detailed enrollment information for the current student in the specified course."
)]
        [ProducesResponseType(typeof(Result<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEnrollmentDetails(int courseId)
        {
            var result = await mediator.Send(new GetEnrollmentDetailsQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }
        [HttpPost("{courseId}/complete")]
        [SwaggerOperation(
    Summary = "Mark course as completed",
    Description = "Marks the specified enrolled course as completed for the current student."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkCourseAsCompleted(int courseId)
        {
            var result = await mediator.Send(new MarkCourseAsCompletedCommand
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }

        [HttpGet("{courseId}/progress")]
        [SwaggerOperation(
    Summary = "Get course progress",
    Description = "Retrieves the current student's progress for the specified enrolled course."
)]
        [ProducesResponseType(typeof(Result<CourseProgressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourseProgress(int courseId)
        {
            var result = await mediator.Send(new GetCourseProgressQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }
        [HttpGet("continue-watching")]
        [SwaggerOperation(
    Summary = "Get continue watching courses",
    Description = "Retrieves courses that the current student started but has not completed yet, with last watched progress."
)]
        [ProducesResponseType(typeof(Result<List<ContinueWatchingDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContinueWatching()
        {
            var result = await mediator.Send(new GetContinueWatchingQuery());

            return result.ToActionResult();
        }
        [HttpGet("recent-courses")]
        [SwaggerOperation(
    Summary = "Get recent courses",
    Description = "Retrieves the most recently accessed courses by the current student."
)]
        [ProducesResponseType(typeof(Result<List<RecentCourseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentCourses()
        {
            var result = await mediator.Send(new GetRecentCoursesQuery());

            return result.ToActionResult();
        }
        [HttpDelete("{courseId}")]
        [SwaggerOperation(
    Summary = "Cancel enrollment",
    Description = "Cancels the student's enrollment in the specified course."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelEnrollment(int courseId)
        {
            var result = await mediator.Send(new CancelEnrollmentCommand
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }
        [HttpGet("completed-courses")]
        [SwaggerOperation(
    Summary = "Get completed courses",
    Description = "Retrieves all courses that the current student has fully completed."
)]
        [ProducesResponseType(typeof(Result<List<CompletedCourseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompletedCourses()
        {
            var result = await mediator.Send(new GetCompletedCoursesQuery());

            return result.ToActionResult();
        }
    }
}
