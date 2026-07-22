using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler
        : IRequestHandler<GetOrdersQuery,
        Result<PaginatedResult<OrderListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<OrderListDto>>> Handle(
            GetOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.Orders.Query()
                .AsNoTracking()
                .Include(x => x.Student)
                .Include(x => x.OrderItems)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.OrderNumber.Contains(search) ||
                    x.Student.FullName.Contains(search) ||
                    x.Student.Email!.Contains(search));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == request.Status.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var orders = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderListDto
                {
                    OrderId = x.Id,
                    OrderNumber = x.OrderNumber,
                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,
                    SubTotal = x.SubTotal,
                    DiscountAmount = x.DiscountAmount,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    CoursesCount = x.OrderItems.Count
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<OrderListDto>>.Success(
                new PaginatedResult<OrderListDto>(orders, request.PageNumber,
                request.PageSize, totalCount));
        }
    }
}
