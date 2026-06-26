using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.ToggleCategoryStatus
{
    public class ToggleCategoryStatusCommandHandler : IRequestHandler<ToggleCategoryStatusCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ToggleCategoryStatusCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(ToggleCategoryStatusCommand request, CancellationToken cancellationToken)
        {
            //if (!currentUserService.IsInRole("Admin"))
            //    return Result<string>.Failure(ResultStatus.Forbidden, "Access denied.");

            var category = await unitOfWork.Categories.Query()
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(c => c.Id == request.Id
                && !c.IsDeleted
                , cancellationToken);
            if(category is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Category not found.");
            if(request.IsActive && category.IsActive)
                return Result<string>.Failure(ResultStatus.Conflict, "Category is already active.");
            if(!request.IsActive && !category.IsActive)
            {
                if(category.Courses.Any())
                    return Result<string>.Failure(ResultStatus.Conflict, "Cannot deactivate category with associated courses.");
                return Result<string>.Failure(ResultStatus.Conflict, "Category is already inactive.");
            }
               
            category.IsActive = request.IsActive;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"Category {(request.IsActive ? "activated" : "deactivated")} successfully.");

        }
    }
}
