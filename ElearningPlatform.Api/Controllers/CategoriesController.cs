using ElearningPlatform.Api.Common.Responses;
using ElearningPlatform.Application.Features.Categories.Commands.BulkDeleteCategory;
using ElearningPlatform.Application.Features.Categories.Commands.BulkRestoreCategory;
using ElearningPlatform.Application.Features.Categories.Commands.BulkToggleCategoryStatus;
using ElearningPlatform.Application.Features.Categories.Commands.CreateCategory;
using ElearningPlatform.Application.Features.Categories.Commands.DeleteCategory;
using ElearningPlatform.Application.Features.Categories.Commands.EditCategory;
using ElearningPlatform.Application.Features.Categories.Commands.EditCategoryIcon;
using ElearningPlatform.Application.Features.Categories.Commands.RemoveCategoryIcon;
using ElearningPlatform.Application.Features.Categories.Commands.RestoreCategory;
using ElearningPlatform.Application.Features.Categories.Commands.ToggleCategoryStatus;
using ElearningPlatform.Application.Features.Categories.Commands.UploadCategoryIcon;
using ElearningPlatform.Application.Features.Categories.Queries.CheckCategoryNameAvailability;
using ElearningPlatform.Application.Features.Categories.Queries.GetAllCategories;
using ElearningPlatform.Application.Features.Categories.Queries.GetAllCategoriesWithPagination;
using ElearningPlatform.Application.Features.Categories.Queries.GetCategoryById;
using ElearningPlatform.Application.Features.Categories.Queries.GetCategoryByName;
using ElearningPlatform.Application.Features.Categories.Queries.GetCategoryStats;
using ElearningPlatform.Application.Features.Categories.Queries.GetCoursesByCategory;
using ElearningPlatform.Application.Features.Categories.Queries.GetTopCategories;
using ElearningPlatform.Application.Features.Categories.Queries.SearchCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ElearningPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [SwaggerOperation(
          Summary = "Get all categories",
          Description = "Retrieves all categories. Admin sees everything, normal users see only active categories."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllCategoriesQuery());
            return result.ToActionResult();
        }


        [HttpGet("{id}")]
        [SwaggerOperation(
    Summary = "Get category by Id",
    Description = "Retrieves category details. Admin sees all info, user sees only active categories."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));
            return result.ToActionResult();
        }


        [HttpGet("pagination")]
        [SwaggerOperation(
           Summary = "Get categories with pagination",
           Description = "Get all categories, paginated. Admin sees all categories, users see only active ones."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllWithPagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCategoriesWithPaginationQuery(pageNumber, pageSize);
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }


        [HttpPost]
        [SwaggerOperation(
    Summary = "Create a new category",
    Description = "Adds a new course category to the platform."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }



        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Edit category",
    Description = "Updates category details including name, description, icon, and active status."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditCategory(int id, [FromBody] EditCategoryCommand command)
        {
           if(id != command.Id)
            {
                return BadRequest("Category ID in URL does not match ID in request body.");
            }
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Soft delete category",
    Description = "Soft deletes a category by marking it inactive and deleted."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("{id}/restore")]
        [SwaggerOperation(
    Summary = "Restore category",
    Description = "Restores a soft-deleted category. Admin only."
)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            var result = await mediator.Send(new RestoreCategoryCommand(id));
            return result.ToActionResult();
        }

        [HttpPatch("{id}/toggle-status")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Activate or deactivate category",
    Description = "Allows admin to activate or deactivate a category."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ToggleStatus(int id, [FromQuery] bool isActive)
        {
            var command = new ToggleCategoryStatusCommand
            {
                Id = id,
                IsActive = isActive
            };

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }
        [HttpGet("search")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
      Summary = "Search categories",
      Description = "Search categories by name or description with pagination."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCategories(
      [FromQuery] string searchTerm,
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10)
        {
            var query = new SearchCategoriesQuery(searchTerm, pageNumber, pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("{id}/stats")]
        [SwaggerOperation(
     Summary = "Get category statistics",
     Description = "Retrieves statistics for a specific category such as courses count and other related metrics."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoryStats(int id)
        {
            var query = new GetCategoryStatsQuery(id);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("{id}/courses")]
        [SwaggerOperation(
    Summary = "Get courses by category",
    Description = "Retrieve all courses under a specific category with pagination. Admins see extra info."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCoursesByCategory(
    int id,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = new GetCoursesByCategoryQuery(id, pageNumber, pageSize);
            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost("{categoryId}/icon")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Upload category icon",
    Description = "Uploads an icon for a specific category."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadCategoryIcon(
    int categoryId,
    IFormFile icon)
        {
         

            var result = await mediator.Send(new UploadCategoryIconCommand { CategoryId = categoryId, FormFile = icon});

            return result.ToActionResult();
        }
        [HttpPut("{categoryId}/icon")]
        [SwaggerOperation(
    Summary = "Edit category icon",
    Description = "Replace the existing icon of a category."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditCategoryIcon(
    int categoryId,
    IFormFile icon)
        {
           

            var result = await mediator.Send(new EditCategoryIconCommand { CategoryId = categoryId,FormFile = icon});

            return result.ToActionResult();
        }

        [HttpDelete("{categoryId}/icon")]
        [SwaggerOperation(
    Summary = "Remove category icon",
    Description = "Deletes the icon associated with a category."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveCategoryIcon(int categoryId)
        {
            var command = new RemoveCategoryIconCommand(categoryId);

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("bulk-delete")]
        [SwaggerOperation(
    Summary = "Bulk delete categories",
    Description = "Soft delete multiple categories at once."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkDeleteCategories(
    [FromBody] BulkDeleteCategoryCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPatch("bulk-toggle-status")]
        [SwaggerOperation(
    Summary = "Bulk toggle category status",
    Description = "Activate or deactivate multiple categories at once."
)]
        public async Task<IActionResult> BulkToggleStatus(
    [FromBody] BulkToggleCategoryStatusCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk-restore")]
        [SwaggerOperation(
    Summary = "Bulk restore categories",
    Description = "Restore multiple soft-deleted categories."
)]
        public async Task<IActionResult> BulkRestore(
    [FromBody] BulkRestoreCategoryCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("by-name")]
        [SwaggerOperation(
    Summary = "Get category by name",
    Description = "Retrieves category details with courses count using category name."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await mediator.Send(new GetCategoryByNameQuery(name));
            return result.ToActionResult();
        }

        [HttpGet("check-name")]
        [SwaggerOperation(
     Summary = "Check category name availability",
     Description = "Checks whether the given category name already exists or is available for use."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckNameAvailability([FromQuery] string name)
        {
            var result = await mediator.Send(new CheckCategoryNameAvailabilityQuery(name));

            return result.ToActionResult();
        }

        [HttpGet("top")]
        [SwaggerOperation(
    Summary = "Get top categories",
    Description = "Retrieves the top categories ordered by number of courses."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTopCategories([FromQuery] int limit = 5)
        {
            var query = new GetTopCategoriesQuery { Limit = limit };

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }
    }
}
