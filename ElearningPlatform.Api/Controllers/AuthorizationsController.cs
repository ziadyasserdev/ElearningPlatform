using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Authorizations.Commands.AddClaimToUser;
using ElearningPlatform.Application.Features.Authorizations.Commands.AddRoleToUser;
using ElearningPlatform.Application.Features.Authorizations.Commands.CreateRole;
using ElearningPlatform.Application.Features.Authorizations.Commands.DeleteRole;
using ElearningPlatform.Application.Features.Authorizations.Commands.EditRole;
using ElearningPlatform.Application.Features.Authorizations.Commands.EditUserClaimValue;
using ElearningPlatform.Application.Features.Authorizations.Commands.RemoveClaimFromUser;
using ElearningPlatform.Application.Features.Authorizations.Commands.RemoveRoleFromUser;
using ElearningPlatform.Application.Features.Authorizations.Commands.UpdateRoleOfUser;
using ElearningPlatform.Application.Features.Authorizations.Queries.GetAllRoles;
using ElearningPlatform.Application.Features.Authorizations.Queries.GetAllRolesOfUser;
using ElearningPlatform.Application.Features.Authorizations.Queries.GetAllRolesWithPagination;
using ElearningPlatform.Application.Features.Authorizations.Queries.GetRoleById;
using ElearningPlatform.Application.Features.Authorizations.Queries.IsRoleExist;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthorizationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // 

        [HttpGet("roles")]
        [SwaggerOperation(
    Summary = "Get all roles",
    Description = "Retrieve a list of all roles in the system."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await mediator.Send(new GetAllRolesQuery());
            return result.ToActionResult();
        }


        [HttpGet("users/{userId}/roles")]
        [SwaggerOperation(
    Summary = "Get user roles",
    Description = "Retrieve all roles assigned to a specific user."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var result = await mediator.Send(new GetAllRolesOfUserQuery(userId));
            return result.ToActionResult();
        }
        [HttpGet("roles-pagination")]
        [SwaggerOperation(
    Summary = "Get roles with pagination",
    Description = "Retrieve roles using pagination parameters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var result = await mediator.Send(new GetAllRolesWithPaginationQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }

        [HttpGet("roles/{id}")]
        [SwaggerOperation(
    Summary = "Get role by ID",
    Description = "Retrieve role details using role ID."
)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await mediator.Send(new GetRoleByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("roles/{id}/exists")]
        [SwaggerOperation(
    Summary = "Check if role exists",
    Description = "Check whether a role exists using its ID."
)]
        public async Task<IActionResult> RoleExists(string id)
        {
            var result = await mediator.Send(new IsRoleExistQuery(id));
            return result.ToActionResult();
        }

        [HttpPost("roles")]
        [SwaggerOperation(
         Summary = "Create role",
         Description = "Create a new role in the system."
     )]
        public async Task<IActionResult> CreateRole([FromQuery] string roleName)
        {
            var result = await mediator.Send(new CreateRoleCommand(roleName));
            return result.ToActionResult();
        }

        [HttpPut("roles/{id}")]
        [SwaggerOperation(
            Summary = "Update role",
            Description = "Update role information using role ID."
        )]
        public async Task<IActionResult> UpdateRole(string id, EditRoleCommand command)
        {
            if (id != command.roleId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("roles/{id}")]
        [SwaggerOperation(
            Summary = "Delete role",
            Description = "Delete a role using role ID."
        )]
        public async Task<IActionResult> DeleteRole(string id, DeleteRoleCommand command)
        {
            if (id != command.RoleId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("users/roles")]
        [SwaggerOperation(
            Summary = "Assign role to user",
            Description = "Assign an existing role to a specific user."
        )]
        public async Task<IActionResult> AssignRoleToUser(AddRoleToUserCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("users/{userId}/roles")]
        [SwaggerOperation(
            Summary = "Remove role from user",
            Description = "Remove a specific role from a user."
        )]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, RemoveRoleFromUserCommand command)
        {
            if (userId != command.userId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("users/{userId}/roles")]
        [SwaggerOperation(
            Summary = "Update user roles",
            Description = "Update roles assigned to a specific user."
        )]
        public async Task<IActionResult> UpdateUserRole(string userId, UpdateRoleOfUserCommand command)
        {
            if (userId != command.UserId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("users/claims")]
        [SwaggerOperation(
            Summary = "Assign claim to user",
            Description = "Assign a claim to a specific user."
        )]
        public async Task<IActionResult> AssignClaimToUser(AddClaimToUserCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("users/{userId}/claims")]
        [SwaggerOperation(
            Summary = "Update user claim",
            Description = "Update claim value assigned to a specific user."
        )]
        public async Task<IActionResult> UpdateUserClaim(string userId, EditUserClaimValueCommand command)
        {
            if (userId != command.UserId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("users/{userId}/claims")]
        [SwaggerOperation(
    Summary = "Remove claim from user",
    Description = "Remove a specific claim from a user."
)]
        public async Task<IActionResult> RemoveClaimFromUser(string userId, RemoveClaimFromUserCommand command)
        {
            if (userId != command.userId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

    }
}
