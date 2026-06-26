using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.AddImageProfile;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.ChangePassword;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteImageProfile;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteMyAccount;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteUserByAdmin;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.RestoreUserByAdmin;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.UpdateImageProfile;
using ElearningPlatform.Application.Features.ApplicationUsers.Commands.UpdateUserProfile;
using ElearningPlatform.Application.Features.ApplicationUsers.Queries.CheckEmailExist;
using ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetAllUsers;
using ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetAllUsersByStatus;
using ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetUserById;
using ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetUserProfile;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApplicationUsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("users")]
        [SwaggerOperation(
    Summary = "Get all users",
    Description = "Retrieve a list of all registered application users."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await mediator.Send(new GetAllUsersQuery());
            return result.ToActionResult();
        }



        [HttpPost("upload-image-profile")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Upload or update user profile image",
            Description = "Allows the authenticated user to upload or update their profile image. " +
                          "The image must be JPG, JPEG, or PNG and should not exceed 2MB."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]       // Image uploaded successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))] // Validation errors (null file, invalid type, size)
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<string>))] // Not logged in
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))] // User not found
        public async Task<IActionResult> AddProfileImage([FromForm] AddImageProfileCommand addImageProfileCommand)
        {
            var result = await mediator.Send(addImageProfileCommand);
            return result.ToActionResult();
        }
        [HttpDelete("image-profile")]
        [Authorize]
        [SwaggerOperation(
  Summary = "Delete authenticated user's profile image",
  Description = "Allows the logged-in user to delete their profile image. " +
                "This does not delete the user account, only the profile image."
)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]       // Image deleted successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))] // Validation error / image not found
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<string>))] // User not authenticated
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))] // User or image not found
        public async Task<IActionResult> DeleteImageProfile()
        {
            var result = await mediator.Send(new DeleteImageProfileCommand());
            return result.ToActionResult();
        }
        [HttpPost("change-password")]
        [SwaggerOperation(
           Summary = "Change user password",
           Description = "Allow a user to change their password by providing old and new password."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            var result = await mediator.Send(changePasswordCommand);
            return result.ToActionResult();
        }
        [HttpDelete("{userId}")]

        [SwaggerOperation(
                   Summary = "Delete user by admin",
                   Description = "Allows an administrator to permanently delete a user account from the system."
               )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserByAdmin(
                   [FromRoute] string userId)
        {
            var command = new DeleteUserByAdminCommand(userId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("my-account")]
        [Authorize]
        [SwaggerOperation(
       Summary = "Delete own user account",
       Description = "Allows the authenticated user to permanently delete their own account."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var result = await mediator.Send(new DeleteMyAccountCommand());

            return result.ToActionResult();
        }

        [HttpPatch("restore-user-by-admin/{userId}")]

        [SwaggerOperation(
           Summary = "Restore a soft-deleted user by admin",
           Description = "Allows an administrator to restore a user account that was previously soft-deleted."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RestoreUserByAdmin([FromRoute] string userId)
        {
            var command = new RestoreUserByAdminCommand(userId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("update-image-profile")]

        [SwaggerOperation(
       Summary = "Update the authenticated user's profile image",
       Description = "Allows the logged-in user to update their profile image. " +
                     "Accepted file types: JPG, JPEG, PNG. Maximum size: 2MB."
   )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]       // Image updated successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<string>))] // Validation error (null file, invalid type, size)
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<string>))] // User not authenticated
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<string>))] // User not found
        public async Task<IActionResult> UpdateImageProfile([FromForm] UpdateImageProfileCommand command)
        {
            if (command.ProfileImage is null)
                return Result<string>.Failure(ResultStatus.ValidationError, "Profile image is required.").ToActionResult();

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }


        [HttpPut("profile")]
        [SwaggerOperation(
    Summary = "Update user profile",
    Description = "Update the profile information of the authenticated user such as full name, phone number, and gender."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileCommand updateUserProfileCommand)
        {
            var result = await mediator.Send(updateUserProfileCommand);
            return result.ToActionResult();
        }
        [HttpGet("check-email/{userEmail}")]
        [SwaggerOperation(
            Summary = "Check if email exists",
            Description = "Checks whether a user account with the specified email address exists in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckEmail(
            [FromRoute] string userEmail)
        {
            var result = await mediator.Send(
                new CheckEmailExistQuery(userEmail));

            return result.ToActionResult();
        }


        [HttpGet("by-status")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Get all users by status",
     Description = "Retrieves a paginated list of users filtered by their status (Active, Inactive, Deleted, etc.)."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsersByStatus(
     [FromQuery] UserStatus status,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
        {
            var query = new GetAllUsersByStatusQuery(pageNumber, pageSize, status);
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Get user by ID",
           Description = "Retrieve details of a specific user by their ID."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await mediator.Send(new GetUserByIdQuery { userId = id });
            return result.ToActionResult();
        }
        [HttpGet("my-profile")]
        [SwaggerOperation(
         Summary = "Get current user's profile",
         Description = "Retrieve the profile details of the authenticated user."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await mediator.Send(new GetUserProfileQuery { });
            return result.ToActionResult();
        }
    }
}
