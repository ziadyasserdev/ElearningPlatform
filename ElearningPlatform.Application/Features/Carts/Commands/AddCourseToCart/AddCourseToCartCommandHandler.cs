using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Commands.AddCourseToCart
{
    public class AddCourseToCartCommandHandler
       : IRequestHandler<AddCourseToCartCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddCourseToCartCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            AddCourseToCartCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");

            var userId = currentUserService.UserId;

            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CourseId &&
                    !x.IsDeleted &&
                    x.IsActive &&
                    x.Status == CourseStatus.Published,
                    cancellationToken);

            if (course is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");

            var alreadyEnrolled = await unitOfWork.Enrollments.Query()
                .AnyAsync(x =>
                    x.CourseId == request.CourseId &&
                    x.StudentId == userId,
                    cancellationToken);

            if (alreadyEnrolled)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "You are already enrolled in this course.");

            var cart = await unitOfWork.Carts.Query()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x =>
                    x.StudentId == userId,
                    cancellationToken);

            if (cart is null)
            {
                cart = new Cart
                {
                    StudentId = userId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = currentUserService.UserName
                };

                await unitOfWork.Carts.AddAsync(cart);
            }

            var alreadyInCart = cart.CartItems
                .Any(x => x.CourseId == request.CourseId);

            if (alreadyInCart)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Course already exists in your cart.");

            cart.CartItems.Add(new CartItem
            {
                CourseId = request.CourseId,
                AddedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
           
            });

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Course added to cart successfully.");
        }
    }
}
