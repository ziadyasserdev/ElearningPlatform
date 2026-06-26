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

namespace ElearningPlatform.Application.Features.Categories.Commands.UploadCategoryIcon
{
    public class UploadCategoryIconCommandHandler : IRequestHandler<UploadCategoryIconCommand, Result<string>>
    {
        private readonly IFileService fileService;
        private readonly IUnitOfWork unitOfWork;

        public UploadCategoryIconCommandHandler(IFileService fileService,IUnitOfWork unitOfWork)
        {
            this.fileService = fileService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(UploadCategoryIconCommand request, CancellationToken cancellationToken)
        {
            if (request.FormFile is null)
                return Result<string>.Failure(ResultStatus.Failure, "Icon file is required.");

            var category = await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.CategoryId && !x.IsDeleted && x.IsActive,
                    cancellationToken);

            if (category is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Category not found or inactive.");

            if (category.IconUrl is not null)
                return Result<string>.Failure(ResultStatus.Conflict,
                    "Category already has an icon. Delete the existing icon first.");

            var uploadResult = await fileService.UploadImageAsync(request.FormFile);

            if (!uploadResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure, uploadResult.Error);

            category.IconUrl = uploadResult.Value;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(uploadResult.Value!, "Icon uploaded successfully.");
        }
    }
}
