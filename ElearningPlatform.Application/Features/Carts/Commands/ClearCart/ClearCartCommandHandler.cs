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

namespace ElearningPlatform.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartCommandHandler
         : IRequestHandler<ClearCartCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ClearCartCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            ClearCartCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");

            var userId = currentUserService.UserId;

            var cart = await unitOfWork.Carts.Query()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.StudentId == userId, cancellationToken);

            if (cart is null || !cart.CartItems.Any())
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Cart is empty.");

            unitOfWork.CartItems.DeleteRange(cart.CartItems);

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Cart cleared successfully.");
        }
    }
}
