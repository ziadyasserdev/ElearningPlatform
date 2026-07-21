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

namespace ElearningPlatform.Application.Features.Carts.Commands.RemoveCourseFromCart
{
    public class RemoveCourseFromCartCommandHandler
        : IRequestHandler<RemoveCourseFromCartCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RemoveCourseFromCartCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RemoveCourseFromCartCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");

            var userId = currentUserService.UserId;

            var cart = await unitOfWork.Carts.Query()
                .FirstOrDefaultAsync(x => x.StudentId == userId, cancellationToken);

            if (cart is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Cart not found");

            var cartItem = await unitOfWork.CartItems.Query()
                .FirstOrDefaultAsync(x =>
                    x.CartId == cart.Id &&
                    x.CourseId == request.CourseId,
                    cancellationToken);

            if (cartItem is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Course not found in cart");

            unitOfWork.CartItems.Delete(cartItem);

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Course removed from cart successfully");
        }
    }
}
