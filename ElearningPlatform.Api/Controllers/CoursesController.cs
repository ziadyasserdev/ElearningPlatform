using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Categories.Queries.GetAllCategoriesWithPagination;
using ElearningPlatform.Application.Features.Courses.Commands.CreateCourse;
using ElearningPlatform.Application.Features.Courses.Commands.DeleteCourse;
using ElearningPlatform.Application.Features.Courses.Commands.DeleteCourseThumbnail;
using ElearningPlatform.Application.Features.Courses.Commands.EditCourse;
using ElearningPlatform.Application.Features.Courses.Commands.FeatureCourse;
using ElearningPlatform.Application.Features.Courses.Commands.PublishCourse;
using ElearningPlatform.Application.Features.Courses.Commands.UnPublishCourse;
using ElearningPlatform.Application.Features.Courses.Commands.UpdateCourseThumbnail;
using ElearningPlatform.Application.Features.Courses.Commands.UploadCourseThumbnail;
using ElearningPlatform.Application.Features.Courses.Queries.GetAllCourses;
using ElearningPlatform.Application.Features.Courses.Queries.GetAllCoursesWithPagination;
using ElearningPlatform.Application.Features.Courses.Queries.GetCourseById;
using ElearningPlatform.Application.Features.Courses.Queries.GetCourseDetails;
using ElearningPlatform.Application.Features.Courses.Queries.GetFeaturedCourses;
using ElearningPlatform.Application.Features.Courses.Queries.GetLatestCourses;
using ElearningPlatform.Application.Features.Courses.Queries.GetPopularCourses;
using ElearningPlatform.Application.Features.Courses.Queries.GetTopCourses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CoursesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
    Summary = "Create a new course",
    Description = "Creates a new course with the provided details."
)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateCourseCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
     Summary = "Update course",
     Description = "Updates an existing course with the provided data."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(int id, [FromBody] EditCourseCommand command)
        {
            command.CourseId = id;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete course",
    Description = "Deletes a course by its ID (soft or hard delete depending on implementation)."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteCourseCommand(id));

            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get all courses",
    Description = "Retrieves all courses without pagination (use carefully in large datasets)."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllCoursesQuery());

            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
    Summary = "Get course by Id",
    Description = "Retrieves a specific course by its unique identifier."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetCourseByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(
       Summary = "Get all courses with pagination",
       Description = "Retrieves courses using pagination. You can التحكم في عدد العناصر باستخدام PageNumber و PageSize."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllWithPagination(
       [FromQuery] int PageNumber,
       [FromQuery] int PageSize)
        {
            var result = await mediator.Send(
                new GetAllCoursesWithPaginationQuery(PageNumber, PageSize));

            return result.ToActionResult();
        }
        [HttpGet("courses/{id}/details")]
        [SwaggerOperation(
    Summary = "Get course details",
    Description = "Returns full course structure (sections, lessons, videos)."
)]
        public async Task<IActionResult> GetCourseDetails(int id)
        {
            var result = await mediator.Send(new GetCourseDetailsQuery { Id = id });
            return result.ToActionResult();
        }
        [HttpPost("courses/{courseId}/publish")]
        [SwaggerOperation(
    Summary = "Publish course",
    Description = "Publishes a course and makes it available for students."
)]
        public async Task<IActionResult> PublishCourse(int courseId)
        {
            var result = await mediator.Send(new PublishCourseCommand
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }

        [HttpPost("courses/{courseId}/unpublish")]
        [SwaggerOperation(
    Summary = "Unpublish course",
    Description = "Moves course back to draft state and hides it from students."
)]
        public async Task<IActionResult> UnPublishCourse(int courseId)
        {
            var result = await mediator.Send(new UnPublishCourseCommand
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }

        [HttpPost("courses/{courseId}/feature")]
        [SwaggerOperation(
    Summary = "Feature course",
    Description = "Marks a course as featured so it appears on homepage or highlighted sections."
)]
        public async Task<IActionResult> FeatureCourse(int courseId)
        {
            var result = await mediator.Send(new FeatureCourseCommand
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }

        [HttpGet("courses/featured")]
        [SwaggerOperation(
    Summary = "Get featured courses",
    Description = "Retrieves all featured courses that are highlighted on the homepage or featured sections."
)]
        [ProducesResponseType(typeof(Result<List<CourseUserDtoo>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeaturedCourses()
        {
            var result = await mediator.Send(new GetFeaturedCoursesQuery());

            return result.ToActionResult();
        }


        [HttpGet("courses/popular")]
        [SwaggerOperation(
    Summary = "Get popular courses",
    Description = "Retrieves the most popular courses based on enrollments, ratings, or platform popularity metrics."
)]
        [ProducesResponseType(typeof(Result<List<CourseUserDtoo>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPopularCourses()
        {
            var result = await mediator.Send(new GetPopularCoursesQuery());

            return result.ToActionResult();
        }
        [HttpGet("courses/latest")]
        [SwaggerOperation(
    Summary = "Get latest courses",
    Description = "Retrieves the latest published courses ordered by creation date."
)]
        [ProducesResponseType(typeof(Result<List<CourseUserDtoo>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLatestCourses()
        {
            var result = await mediator.Send(new GetLatestCoursesQuery());

            return result.ToActionResult();
        }


        [HttpPost("courses/{courseId}/thumbnail")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
    Summary = "Upload course thumbnail",
    Description = "Uploads or updates the thumbnail image for a course."
)]
        public async Task<IActionResult> UploadCourseThumbnail(
    int courseId,
    IFormFile file)
        {
            var command = new UploadCourseThumbnailCommand
            {
                CourseId = courseId,
                File = file
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("courses/{courseId}/thumbnail")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
    Summary = "Update course thumbnail",
    Description = "Replaces the existing thumbnail image of a course."
)]
        public async Task<IActionResult> UpdateCourseThumbnail(
    int courseId,
     IFormFile file)
        {
            var command = new UpdateCourseThumbnailCommand
            {
                CourseId = courseId,
                File = file
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("courses/{courseId}/thumbnail")]
        [SwaggerOperation(
    Summary = "Delete course thumbnail",
    Description = "Removes the thumbnail image associated with a course."
)]
        public async Task<IActionResult> DeleteCourseThumbnail(int courseId)
        {
            var result = await mediator.Send(new DeleteCourseThumbnailCommand
            {
                CourseId = courseId
            });

            return result.ToActionResult();
        }


        [HttpGet("top")]
        [SwaggerOperation(
            Summary = "Get top courses",
            Description = "Retrieves top performing courses based on enrollments and completion metrics."
        )]
        [ProducesResponseType(typeof(Result<List<TopCourseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopCourses([FromQuery] int take = 10)
        {
            var result = await mediator.Send(new GetTopCoursesQuery
            {
                Take = take
            });

            return result.ToActionResult();
        }
    }
}
