using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Videos.Commands.BulkDeleteVideos;
using ElearningPlatform.Application.Features.Videos.Commands.BulkRestoreVideos;
using ElearningPlatform.Application.Features.Videos.Commands.CreateVideo;
using ElearningPlatform.Application.Features.Videos.Commands.CreateVideoComment;
using ElearningPlatform.Application.Features.Videos.Commands.DeleteThumbnail;
using ElearningPlatform.Application.Features.Videos.Commands.DeleteVideo;
using ElearningPlatform.Application.Features.Videos.Commands.DeleteVideoComment;
//using ElearningPlatform.Application.Features.Videos.Commands.GetVideoProgress;
using ElearningPlatform.Application.Features.Videos.Commands.ReorderVideos;
using ElearningPlatform.Application.Features.Videos.Commands.ReplyVideoComment;
using ElearningPlatform.Application.Features.Videos.Commands.RestoreVideo;
using ElearningPlatform.Application.Features.Videos.Commands.UpdateThumbnail;
using ElearningPlatform.Application.Features.Videos.Commands.UpdateVideo;
using ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoComment;
using ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoProgress;
using ElearningPlatform.Application.Features.Videos.Commands.UploadThumbnail;
using ElearningPlatform.Application.Features.Videos.Queries.DownloadVideo;
using ElearningPlatform.Application.Features.Videos.Queries.GetCommentsByTimestamp;
using ElearningPlatform.Application.Features.Videos.Queries.GetCommentThread;
using ElearningPlatform.Application.Features.Videos.Queries.GetVideoById;
using ElearningPlatform.Application.Features.Videos.Queries.GetVideoComments;
using ElearningPlatform.Application.Features.Videos.Queries.GetVideoProgress;
using ElearningPlatform.Application.Features.Videos.Queries.GetVideosByLessonId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IMediator mediator;

        public VideosController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("lessons/{lessonId}/videos")]
        [SwaggerOperation(
           Summary = "Upload video",
           Description = "Uploads a video file and attaches it to a lesson"
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateVideo(
           [FromRoute] int lessonId,
           [FromForm] CreateVideoCommand command)
        {


            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet]
        [Route("lessons/{lessonId}/videos/{videoId}/download")]
        [SwaggerOperation(
   Summary = "Download video",
   Description = "Downloads a video file from a lesson"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadVideo(
    [FromRoute] int lessonId,
    [FromRoute] int videoId)
        {
            var result = await mediator.Send(new DownloadVideoQuery
            {
                LessonId = lessonId,
                VideoId = videoId
            });
            if (!result.IsSuccess)
                return result.ToActionResult();
            var file = result.Value;

            return File(file.FileBytes, file.ContentType, file.FileName);
        }

        [HttpGet]
        [Route("videos/{id}")]
        [SwaggerOperation(
     Summary = "Get video by Id",
     Description = "Retrieves video details by its Id"
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVideo([FromRoute] int id)
        {
            var result = await mediator.Send(new GetVideoByIdQuery { Id = id });

            return result.ToActionResult();
        }
        [HttpPut("videos/{id}")]
        [SwaggerOperation(
    Summary = "Update video",
    Description = "Updates video details and optionally replaces the file"
)]
        public async Task<IActionResult> UpdateVideo(
    [FromRoute] int id,
    [FromForm] UpdateVideoCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpDelete]
        [Route("videos/{id}")]
        [SwaggerOperation(
    Summary = "Delete video",
    Description = "Deletes a video by its Id"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVideo([FromRoute] int id)
        {
            var result = await mediator.Send(new DeleteVideoCommand { Id = id });

            return result.ToActionResult();
        }
        [HttpGet]
        [Route("lessons/{lessonId}/videos")]
        [SwaggerOperation(
    Summary = "Get videos by lesson",
    Description = "Retrieves all videos for a specific lesson"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVideosByLesson([FromRoute] int lessonId)
        {
            var result = await mediator.Send(new GetVideosByLessonIdQuery
            {
                LessonId = lessonId
            });

            return result.ToActionResult();
        }
        [HttpPost]
        [Route("videos/{videoId}/thumbnail")]
        [SwaggerOperation(
    Summary = "Upload video thumbnail",
    Description = "Uploads or replaces the thumbnail image for a specific video"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadThumbnail(
    [FromRoute] int videoId,
     IFormFile thumbnail)
        {
            var command = new UploadThumbnailCommand
            {
                VideoId = videoId,
                Thumbnail = thumbnail
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("videos/{id}/thumbnail")]
        [SwaggerOperation(
       Summary = "Update video thumbnail",
       Description = "Replaces the existing thumbnail image for a specific video. Only the video instructor is allowed to perform this action."
   )]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateThumbnail(
       [FromRoute] int id,
        IFormFile thumbnail)
        {
            if (thumbnail == null || thumbnail.Length == 0)
                return BadRequest("Thumbnail is required");

            var command = new UpdateThumbnailCommand
            {
                VideoId = id,
                Thumbnail = thumbnail
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("videos/{id}/thumbnail")]
        [SwaggerOperation(
    Summary = "Delete video thumbnail",
    Description = "Removes the thumbnail image of a specific video. Only the instructor who owns the video can perform this action."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteThumbnail([FromRoute] int id)
        {
            var command = new DeleteThumbnailCommand
            {
                VideoId = id
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPut("sections/{sectionId}/videos/reorder")]
        [SwaggerOperation(
    Summary = "Reorder videos in a section",
    Description = "Updates the order of videos inside a section"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReorderVideos(
    [FromRoute] int sectionId,
    [FromBody] ReorderVideosCommand command)
        {
            command.SectionId = sectionId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("videos/{id}/restore")]
        [SwaggerOperation(
    Summary = "Restore deleted video",
    Description = "Restores a soft-deleted video back to active state"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreVideo([FromRoute] int id)
        {
            var command = new RestoreVideoCommand
            {
                VideoId = id
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("videos/bulk-restore")]
        [SwaggerOperation(
    Summary = "Bulk restore videos",
    Description = "Restores multiple soft-deleted videos at once"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkRestoreVideos([FromBody] BulkRestoreVideosCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("videos/bulk")]
        [SwaggerOperation(
    Summary = "Bulk delete videos",
    Description = "Soft deletes multiple videos at once"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkDeleteVideos([FromBody] BulkDeleteVideosCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("videos/{videoId}/progress")]
        [SwaggerOperation(
    Summary = "Update video progress",
    Description = "Tracks the current watch position of a user inside a video and updates progress percentage"
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateProgress(
    [FromRoute] int videoId,
    [FromBody] UpdateVideoProgressCommand command)
        {
            command.VideoId = videoId;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("videos/{videoId}/progress")]
        [SwaggerOperation(
     Summary = "Get video progress",
     Description = "Retrieves the current watch progress of a user for a specific video including resume position and completion status"
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetProgress([FromRoute] int videoId)
        {
            var result = await mediator.Send(new GetVideoProgressQuery
            {
                VideoId = videoId
            });

            return result.ToActionResult();
        }

        [HttpPost("{videoId}/comments")]
        [SwaggerOperation(
        Summary = "Create video comment",
        Description = "Adds a new comment to a specific video. Supports optional timestamp for video-linked comments."
    )]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateComment(
        int videoId,
        [FromBody] CreateVideoCommentCommand command)
        {
            command.VideoId = videoId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("{videoId}/comments")]
        [SwaggerOperation(
       Summary = "Get video comments",
       Description = "Retrieves all comments for a video including nested replies."
   )]
        [ProducesResponseType(typeof(Result<List<VideoCommentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVideoComments(int videoId)
        {
            var result = await mediator.Send(new GetVideoCommentsQuery
            {
                VideoId = videoId
            });

            return result.ToActionResult();
        }
        [HttpPost("{videoId}/comments/{commentId}/reply")]
        [SwaggerOperation(
       Summary = "Reply to video comment",
       Description = "Adds a reply to an existing video comment."
   )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReplyToComment(
       int videoId,
       int commentId,
       [FromBody] ReplyVideoCommentCommand command)
        {
            command.VideoId = videoId;
            command.CommentId = commentId;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPut("comments/{id}")]
        [SwaggerOperation(
     Summary = "Update video comment",
     Description = "Updates the content of an existing video comment."
 )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateVideoCommentCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpDelete("comments/{id}")]
        [SwaggerOperation(
       Summary = "Delete video comment",
       Description = "Deletes a video comment by its Id."
   )]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await mediator.Send(new DeleteVideoCommentCommand
            {
                Id = id
            });

            return result.ToActionResult();
        }
        [HttpGet("comments/{id}/thread")]
        [SwaggerOperation(
     Summary = "Get comment thread",
     Description = "Retrieves a full comment thread including all nested replies."
 )]
        [ProducesResponseType(typeof(Result<VideoCommentThreadDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentThread(int id)
        {
            var result = await mediator.Send(new GetCommentThreadQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }


        [HttpGet("{videoId}/comments/by-timestamp")]
        [SwaggerOperation(
            Summary = "Get comments by timestamp",
            Description = "Retrieves comments for a video filtered by specific timestamp (in seconds)."
        )]
        [ProducesResponseType(typeof(Result<List<VideoCommentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentsByTimestamp(
            int videoId,
            [FromQuery] int? timestamp)
        {
            var result = await mediator.Send(new GetCommentsByTimestampQuery
            {
                VideoId = videoId,
                Timestamp = timestamp
            });

            return result.ToActionResult();
        }
        [HttpGet("{videoId}/progress")]
        [SwaggerOperation(
      Summary = "Get video progress",
      Description = "Retrieves the user's watch progress for a specific video."
  )]
        [ProducesResponseType(typeof(Result<VideoProgressDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVideoProgress(int videoId)
        {
            var result = await mediator.Send(new GetVideoProgressQuery
            {
                VideoId = videoId
            });

            return result.ToActionResult();
        }
    }
}

