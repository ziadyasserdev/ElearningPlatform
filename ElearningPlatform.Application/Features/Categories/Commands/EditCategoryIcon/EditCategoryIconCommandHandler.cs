using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.EditCategoryIcon
{
    public class EditCategoryIconCommandHandler : IRequestHandler<EditCategoryIconCommand, Result<string>>
    {
        private readonly IFileService fileService;
        private readonly IUnitOfWork unitOfWork;

        public EditCategoryIconCommandHandler(IFileService fileService, IUnitOfWork unitOfWork)
        {
            this.fileService = fileService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(EditCategoryIconCommand request, CancellationToken cancellationToken)
        {
            if (request.FormFile is null)
                return Result<string>.Failure(ResultStatus.Failure, "Icon file is required.");

            var category = await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(x => x.Id == request.CategoryId &&
                                          !x.IsDeleted &&
                                          x.IsActive,
                                          cancellationToken);

            if (category is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Category not found or inactive.");

            if (category.IconUrl is null)
                return Result<string>.Failure(ResultStatus.NotFound,
                    "Category does not have an icon to edit.");

            
            var uploadResult = await fileService.UploadImageAsync(request.FormFile);

            if (!uploadResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure, uploadResult.Error);

           
            var removeResult = fileService.Remove(category.IconUrl);

            if (!removeResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure,
                    "Failed to remove existing icon: " + removeResult.Error);

            category.IconUrl = uploadResult.Value;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(uploadResult.Value!, "Icon updated successfully.");
        }
    }
}
