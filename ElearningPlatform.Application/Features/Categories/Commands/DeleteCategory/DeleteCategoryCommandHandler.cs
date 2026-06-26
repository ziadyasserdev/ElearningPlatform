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

namespace ElearningPlatform.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
     .Include(c => c.Courses)
     .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);
            if (category is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found.");
          if(category.Courses.Any())
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot delete category with associated courses.");
            category.IsDeleted = true;
            category.IsActive = false;
            category.DeletedAt = DateTime.Now;
            category.DeletedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(category.Id);
        }
    }
}
