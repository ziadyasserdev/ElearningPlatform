using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Carts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Queries.GetMyCart
{
    public class GetMyCartQueryHandler
    : IRequestHandler<GetMyCartQuery, Result<CartResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyCartQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<CartResponse>> Handle(
            GetMyCartQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<CartResponse>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            var userId = currentUserService.UserId;

            var cart = await unitOfWork.Carts.Query()
                .Where(x => x.StudentId == userId)
                .Select(x => new CartResponse
                {
                    TotalItems = x.CartItems.Count,

                    Items = x.CartItems.Select(i => new CartItemDto
                    {
                        CourseId = i.CourseId,

                        Title = i.Course.Title,

                        ThumbnailUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}" +
                $"://{_httpContextAccessor.HttpContext!.Request.Host}" +
                $"/{i.Course.ThumbnailUrl}",

                        InstructorName = i.Course.Instructor.User.FullName,

                        Price = i.Course.Price,

                        DiscountPrice = i.Course.DiscountPrice,

                        FinalPrice = i.Course.DiscountPrice ?? i.Course.Price

                    }).ToList()

                })
                .FirstOrDefaultAsync(cancellationToken);

            if (cart is null)
                return Result<CartResponse>.Success(new CartResponse());

            cart.SubTotal = cart.Items.Sum(x => x.FinalPrice);

            cart.CourseDiscount = cart.Items.Sum(x => x.Price - x.FinalPrice);

         
            cart.CouponDiscount = 0;

            cart.Total = cart.SubTotal - cart.CouponDiscount;

            return Result<CartResponse>.Success(cart);
        }
    }
}