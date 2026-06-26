using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.RemoveCategoryIcon
{
    public class RemoveCategoryIconCommandHandler : IRequestHandler<RemoveCategoryIconCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;

        public RemoveCategoryIconCommandHandler(IUnitOfWork unitOfWork,IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        public async Task<Result<string>> Handle(RemoveCategoryIconCommand request, CancellationToken cancellationToken)
        {

            var category = await unitOfWork.Categories.Query()
        .FirstOrDefaultAsync(
            x => x.Id == request.CategoryId && !x.IsDeleted && x.IsActive,
            cancellationToken);

            if (category is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Category not found or inactive.");

            if (string.IsNullOrEmpty(category.IconUrl))
                return Result<string>.Failure(ResultStatus.NotFound, "No icon is associated with this category.");

            var removeResult = fileService.Remove(category.IconUrl);

            if (!removeResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure,
                    $"Failed to remove existing icon: {removeResult.Error}");

            category.IconUrl = null;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Icon removed successfully.");

        }
    }
}
