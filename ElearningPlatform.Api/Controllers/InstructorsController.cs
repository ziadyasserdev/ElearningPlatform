using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Commands.AddInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.BecomeInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.BulkDeleteInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.BulkRestoreInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.BulkToggleInstructorStatus;
using ElearningPlatform.Application.Features.Instructors.Commands.DeleteInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.DeleteProfileImage;
using ElearningPlatform.Application.Features.Instructors.Commands.EditInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.RestoreInstructor;
using ElearningPlatform.Application.Features.Instructors.Commands.ToggleInstructorStatus;
using ElearningPlatform.Application.Features.Instructors.Commands.UpdateProfileImage;
using ElearningPlatform.Application.Features.Instructors.Commands.UploadProfileImage;
using ElearningPlatform.Application.Features.Instructors.Queries.CheckIfUserIsInstructor;
using ElearningPlatform.Application.Features.Instructors.Queries.GetAllInstructors;
using ElearningPlatform.Application.Features.Instructors.Queries.GetAllInstructorsWithPagination;
using ElearningPlatform.Application.Features.Instructors.Queries.GetEnrollmentStats;
using ElearningPlatform.Application.Features.Instructors.Queries.GetInactiveStudents;
using ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorById;
using ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorCourses;
using ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorDashboard;
using ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorReviews;
using ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorStudentsCount;
using ElearningPlatform.Application.Features.Instructors.Queries.GetStudentsInCourse;
using ElearningPlatform.Application.Features.Instructors.Queries.GetTopInstructors;
using ElearningPlatform.Application.Features.Instructors.Queries.GetTopStudents;
using ElearningPlatform.Application.Features.Instructors.Queries.SearchInstructors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IMediator mediator;

        public InstructorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
    Summary = "Create a new instructor",
    Description = "Creates a new instructor account with user info and instructor profile."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateInstructor([FromBody] AddInstructorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ================= EDIT =================
        [HttpPut]
        [SwaggerOperation(
            Summary = "Edit instructor profile",
            Description = "Updates the instructor profile. Only bio, specialization, experience, LinkedIn, and website are editable."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditInstructor([FromBody] EditInstructorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ================= DELETE =================
        [HttpDelete("{userId}")]
        [SwaggerOperation(
            Summary = "Delete instructor",
            Description = "Soft deletes the instructor by setting IsDeleted = true and IsActive = false."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInstructor(string userId)
        {
            var command = new DeleteInstructorCommand { UserId = userId };
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all instructors",
            Description = "Retrieves a list of all active instructors with their profile details."
        )]

        public async Task<IActionResult> GetAll()
        {

            var result = await mediator.Send(new GetAllInstructorsQuery());
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get instructor by Id",
            Description = "Retrieves instructor details using the instructor Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInstructorById(int id)
        {
            var query = new GetInstructorByIdQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("paginated")]
        [SwaggerOperation(
      Summary = "Get instructors with pagination",
      Description = "Retrieves a paginated list of instructors based on the provided page number and page size."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllWithPagination(
      [FromQuery] int pageNumber,
      [FromQuery] int pageSize)
        {
            var query = new GetAllInstructorsWithPaginationQuery(pageSize, pageNumber);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("search")]
        [SwaggerOperation(
     Summary = "Search instructors",
     Description = "Search instructors based on name, specialization, or other filters."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] SearchInstructorsQuery query)
        {
            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("top")]
        [SwaggerOperation(
    Summary = "Get top instructors",
    Description = "Retrieves the top instructors based on rating, popularity, or other ranking criteria."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTop([FromQuery] int count = 5)
        {
            var query = new GetTopInstructorsQuery(count);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }


        [HttpGet("{id}/dashboard")]
        [SwaggerOperation(
     Summary = "Get instructor dashboard",
     Description = "Retrieves dashboard data for a specific instructor, including stats, courses, and performance metrics."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDashboard(int id)
        {
            var query = new GetInstructorDashboardQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPatch("{userId}/status")]
        [SwaggerOperation(
    Summary = "Activate or deactivate instructor",
    Description = "Allows admin to activate or deactivate an instructor account."
)]
        public async Task<IActionResult> ToggleStatus(string userId, [FromQuery] bool isActive)
        {
            var command = new ToggleInstructorStatusCommand
            {
                UserId = userId,
                IsActive = isActive
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }




        [HttpPatch("{userId}/restore")]
        [SwaggerOperation(
    Summary = "Restore instructor",
    Description = "Restores a soft-deleted instructor account."
)]
        public async Task<IActionResult> RestoreInstructor(string userId)
        {
            var result = await mediator.Send(new RestoreInstructorCommand(userId));
         

            return result.ToActionResult();
        }






        [HttpGet("{id}/courses")]
        [SwaggerOperation(
Summary = "Get instructor courses",
Description = "Retrieves all courses created by a specific instructor."
)]
        public async Task<IActionResult> GetInstructorCourses(int id)
        {
            var result = await mediator.Send(new GetInstructorCoursesQuery(id));
            return result.ToActionResult();
        }

        [HttpPost("upload-image")]
        [SwaggerOperation(
    Summary = "Upload profile image",
    Description = "Allows the authenticated user to upload or update their profile image."
)]
        public async Task<IActionResult> UploadImage( IFormFile formFile)
        {
            var result = await mediator.Send(new UploadProfileImageCommand
            {
                Image = formFile
            });

            return result.ToActionResult();
        }


        [HttpPost("update-image")]
        [SwaggerOperation(
  Summary = "Update profile image",
  Description = "Allows the authenticated user to update their profile image."
)]
        public async Task<IActionResult> UpdateImage(IFormFile formFile)
        {
            var result = await mediator.Send(new UpdateProfileImageCommand
            {
                formFile = formFile
            });

            return result.ToActionResult();
        }


        [HttpDelete("delete-image")]
        [SwaggerOperation(
  Summary = "Delete profile image",
  Description = "Allows the authenticated user to delete their profile image."
)]
        public async Task<IActionResult> DeleteImage()
        {
            var result = await mediator.Send(new DeleteProfileImageCommand());

            return result.ToActionResult();
        }

        [HttpPost("bulk-toggle-status")]
        [SwaggerOperation(
         Summary = "Activate or deactivate multiple instructors",
         Description = "Allows the admin to bulk activate or deactivate instructors by providing a list of instructor IDs."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // [FromBody] BulkToggleInstructorStatusCommand command
        public async Task<IActionResult> BulkToggleStatus(List<string>ids,bool status)
        {
            var result = await mediator.Send(new BulkToggleInstructorStatusCommand { IsActive = status,UserIds = ids});
            return result.ToActionResult();
        }

        [HttpPost("bulk-delete")]
        [SwaggerOperation(
          Summary = "Soft delete multiple instructors",
          Description = "Allows the admin to soft delete multiple instructors by providing a list of instructor IDs."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] BulkDeleteInstructorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("bulk-restore")]
        [SwaggerOperation(
    Summary = "Restore multiple instructors",
    Description = "Allows the admin to restore soft deleted instructors by providing a list of instructor IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkRestore([FromBody] BulkRestoreInstructorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("{id}/reviews")]
        [SwaggerOperation(
    Summary = "Get instructor reviews",
    Description = "Retrieves all student reviews for a specific instructor."
)]
        public async Task<IActionResult> GetInstructorReviews(int id)
        {
            var query = new GetInstructorReviewsQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }


        [HttpPost("become-instructor")]
        [Authorize]
        [SwaggerOperation(
    Summary = "Become an instructor",
    Description = "Allows a normal user to become an instructor by creating an instructor profile."
)]
        public async Task<IActionResult> BecomeInstructor([FromBody] BecomeInstructorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }


        [HttpGet("is-instructor")]
        [Authorize]
        [SwaggerOperation(
    Summary = "Check if current user is instructor",
    Description = "Returns true if the authenticated user has an instructor profile."
)]
        public async Task<IActionResult> IsInstructor()
        {
            var result = await mediator.Send(new CheckIfUserIsInstructorQuery());
            return result.ToActionResult();
        }

        [HttpGet("{id}/students-count")]
        [SwaggerOperation(
    Summary = "Get instructor students count",
    Description = "Returns the number of unique students enrolled in the instructor's courses."
)]
        public async Task<IActionResult> GetStudentsCount(int id)
        {
            var result = await mediator.Send(new GetInstructorStudentsCountQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("{courseId}/students")]
        [SwaggerOperation(
     Summary = "Get students in course",
     Description = "Retrieves all students enrolled in a specific course for instructor dashboard."
 )]
        [ProducesResponseType(typeof(Result<List<CourseStudentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentsInCourse(int courseId)
        {
            var result = await mediator.Send(new GetStudentsInCourseQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }

        [HttpGet("{courseId}/enrollment-stats")]
        [SwaggerOperation(
    Summary = "Get enrollment statistics",
    Description = "Returns enrollment statistics for a specific course."
)]
        [ProducesResponseType(typeof(Result<EnrollmentStatsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEnrollmentStats(int courseId)
        {
            var result = await mediator.Send(new GetEnrollmentStatsQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }
        [HttpGet("{courseId}/top-students")]
        [SwaggerOperation(
    Summary = "Get top students",
    Description = "Retrieves top performing students in a specific course based on progress and score."
)]
        [ProducesResponseType(typeof(Result<List<TopStudentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopStudents(int courseId)
        {
            var result = await mediator.Send(new GetTopStudentsQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }
        [HttpGet("{courseId}/inactive-students")]
        [SwaggerOperation(
      Summary = "Get inactive students",
      Description = "Retrieves students who are enrolled but have low or no recent activity in the course."
  )]
        [ProducesResponseType(typeof(Result<List<InactiveStudentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInactiveStudents(int courseId)
        {
            var result = await mediator.Send(new GetInactiveStudentsQuery
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }
    }
}




