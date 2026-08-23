using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Commands.CloseExpiredAssignments
{
    public class CloseExpiredAssignmentsHandler : IRequestHandler<CloseExpiredAssignmentsCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CloseExpiredAssignmentsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(CloseExpiredAssignmentsCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;

            var expiredAssignments =
                await unitOfWork.Assignments
                    .Query()
                    .Where(a =>
                        !a.IsDeleted &&
                        a.IsPublished &&
                        !a.IsClosed &&
                        a.DueDate <= now)
                    .ToListAsync(cancellationToken);

            foreach (var assignment in expiredAssignments)
            {
                assignment.IsClosed = true;
                assignment.ClosedAt = now;
                assignment.ClosedBy = "System";
                assignment.UpdatedAt = now;
            }

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                expiredAssignments.Count);
        }
    }
}
