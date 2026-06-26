using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.CheckCategoryNameAvailability
{
    public class CheckCategoryNameAvailabilityQueryHandler : IRequestHandler<CheckCategoryNameAvailabilityQuery, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CheckCategoryNameAvailabilityQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(CheckCategoryNameAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var exists = await unitOfWork.Categories.Query()
                 .AsNoTracking()
                 .AnyAsync(x =>
                     !x.IsDeleted &&
                     x.Name.ToLower() == request.Name.ToLower(),
                     cancellationToken);

            return Result<bool>.Success(!exists);

        }
    }
}
    