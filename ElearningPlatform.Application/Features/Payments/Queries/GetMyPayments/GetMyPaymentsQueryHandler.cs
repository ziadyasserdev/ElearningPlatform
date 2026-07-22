using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Payments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Payments.Queries.GetMyPayments
{
    public class GetMyPaymentsQueryHandler
     : IRequestHandler<GetMyPaymentsQuery,
     Result<PaginatedResult<PaymentListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyPaymentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<PaymentListDto>>> Handle(
            GetMyPaymentsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<PaymentListDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "You must be logged in to view your payments.");
            }


            var query = unitOfWork.Payments.Query()
                .AsNoTracking()
                .Include(x => x.Order)
                .Where(x => x.Order.StudentId == currentUserService.UserId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Order.OrderNumber.Contains(search) ||
                    x.TransactionId!.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var payments = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PaymentListDto
                {
                    PaymentId = x.Id,
                    OrderId = x.OrderId,
                    OrderNumber = x.Order.OrderNumber,
                    Amount = x.Amount,
                    Currency = x.Currency,
                    Provider = x.Provider,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    PaidAt = x.PaidAt
                })
                .ToListAsync(cancellationToken);

           var result = new PaginatedResult<PaymentListDto>(
                payments,
               
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<PaymentListDto>>
                .Success(result);
        }
    }
}
