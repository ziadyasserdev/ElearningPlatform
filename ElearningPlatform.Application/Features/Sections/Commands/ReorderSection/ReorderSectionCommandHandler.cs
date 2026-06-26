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

namespace ElearningPlatform.Application.Features.Sections.Commands.ReorderSection
{
    public class ReorderSectionCommandHandler : IRequestHandler<ReorderSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReorderSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(ReorderSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var section = await unitOfWork.Sections.Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == request.SectionId && !x.IsDeleted, cancellationToken);

            if (section is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Section not found");

          
            if (section.Course.Instructor.UserId != userId
               )
            return Result<int>.Failure(ResultStatus.Unauthorized, "You are not authorized to reorder this section.");

            var currentOrder = section.OrderIndex;
            var newOrder = request.NewOrder;

            if (newOrder <= 0)
                return Result<int>.Failure(ResultStatus.Failure, "Invalid order");

            if (currentOrder == newOrder)
                return Result<int>.Success(section.Id, "No changes");

            
            var sections = unitOfWork.Sections.Query()
                .Where(x => x.CourseId == section.CourseId && !x.IsDeleted);

          
            if (newOrder < currentOrder)
            {
                await sections
                    .Where(x => x.OrderIndex >= newOrder && x.OrderIndex < currentOrder)
                    .ExecuteUpdateAsync(x => x
                        .SetProperty(s => s.OrderIndex, s => s.OrderIndex + 1),
                        cancellationToken);
            }
          
            else
            {
                await sections
                    .Where(x => x.OrderIndex <= newOrder && x.OrderIndex > currentOrder)
                    .ExecuteUpdateAsync(x => x
                        .SetProperty(s => s.OrderIndex, s => s.OrderIndex - 1),
                        cancellationToken);
            }

         
            section.OrderIndex = newOrder;
            section.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(section.Id, "Section reordered successfully");
        }
    }
}
