using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.RestoreCategory
{
    public class RestoreCategoryCommandHandler : IRequestHandler<RestoreCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreCategoryCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted, cancellationToken);
            if(category is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found or not deleted.");
            if(await unitOfWork.Categories.Query()
                .AnyAsync(c => c.Name == category.Name && !c.IsDeleted, cancellationToken))
                return Result<int>.Failure(ResultStatus.Conflict, "A category with the same name already exists.");
            category.IsDeleted = false;
            category.DeletedAt = null;
            category.DeletedBy = null;
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(category.Id);
        }
    }
}
