using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Authentications.Commands.ConfirmEmail;
using ElearningPlatform.Application.Features.Authentications.Commands.EnableTwoFactor;
using ElearningPlatform.Application.Features.Authentications.Commands.ForgetPassword;
using ElearningPlatform.Application.Features.Authentications.Commands.LockUser;
using ElearningPlatform.Application.Features.Authentications.Commands.Login;
using ElearningPlatform.Application.Features.Authentications.Commands.Register;
using ElearningPlatform.Application.Features.Authentications.Commands.ResendConfirmation;
using ElearningPlatform.Application.Features.Authentications.Commands.ResetPassword;
using ElearningPlatform.Application.Features.Authentications.Commands.UnlockUser;
using ElearningPlatform.Application.Features.Authentications.Commands.VerifyTwoFactor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("register")]
        [SwaggerOperation(
        Summary = "Register a new user",
        Description = "Create a new user account with full name, email, password, and other required details."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost("login")]
        [SwaggerOperation(
           Summary = "Login user",
           Description = "Authenticate a user with email and password and return an access token."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await mediator.Send(command);
            //if (result.IsSuccess && !string.IsNullOrEmpty(result.Value?.RefreshToken))
            //{
            //    SetRefreshTokenInCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
            //}
            return result.ToActionResult();
        }
        [HttpGet("confirm-email")]
        [SwaggerOperation(
    Summary = "Confirm user email",
    Description = "Confirms the user's email address using the provided userId and confirmation token."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId,
  [FromQuery] string token)
        {
            var result = await mediator.Send(
        new ConfirmEmailCommand
        {
            UserId = userId,
            Token = token
        });
            return result.ToActionResult();
        }
        [HttpPost("enable-two-factor")]
        [SwaggerOperation(
         Summary = "Enable two factor authentication",
         Description = "Enable two-factor authentication for a specific user."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnableTwoFactor([FromBody] EnableTwoFactorCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPost("forget-password")]
        [SwaggerOperation(
       Summary = "Send password reset link",
       Description = "Send a password reset link to the user's email if the account exists."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Email))
                return BadRequest("Email is required.");

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPost("{userId}/lock")]
        [SwaggerOperation(
   Summary = "Lock a user account",
   Description = "Locks the specified user account, preventing the user from logging in. Accessible only by Admins or Librarians."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LockUser(
   [SwaggerParameter(Description = "The unique identifier of the user to lock.")]
    string userId)
        {
            var result = await mediator.Send(new LockUserCommand(userId));

            return result.ToActionResult();
        }
        [AllowAnonymous]
        [HttpPost("resend-confirmation")]
        [SwaggerOperation(
     Summary = "Resend email confirmation link",
     Description = "Sends a new email confirmation link to the specified email address if the account exists and is not yet confirmed."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost("reset-password")]
        [SwaggerOperation(
          Summary = "Reset user password",
          Description = "Reset the user's password using the token sent by email."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            if (command == null || string.IsNullOrWhiteSpace(command.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(command.Token))
                return BadRequest("Reset token is required.");

            if (string.IsNullOrWhiteSpace(command.NewPassword))
                return BadRequest("New password is required.");

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpPost("{userId}/unlock")]
        [SwaggerOperation(
   Summary = "Unlock a user account",
   Description = "Unlocks the specified user account, allowing the user to log in again. Accessible only by Admins or Librarians."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnlockUser(
   [SwaggerParameter(Description = "The unique identifier of the user to unlock.")]
    string userId)
        {
            var result = await mediator.Send(new UnlockUserCommand(userId));

            return result.ToActionResult();
        }

        [HttpPost("verify-two-factor")]
        [SwaggerOperation(
   Summary = "Verify two-factor authentication code",
   Description = "Verifies the two-factor authentication code provided by the user during login."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyTwoFactor(VerifyTwoFactorCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
    }
}

