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

namespace ElearningPlatform.Application.Features.Assignments.Commands.CreateAssignment
{
    public class CreateAssignmentCommandHandler
       : IRequestHandler<CreateAssignmentCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateAssignmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateAssignmentCommand request,
            CancellationToken cancellationToken)
        {
          

            var course = await _unitOfWork.Courses.Query()
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c =>
                    c.Id == request.CourseId &&
                    !c.IsDeleted &&
                    c.IsActive,
                    cancellationToken);

            if (course is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Course not found.");
            }

          
            if (course.Status != CourseStatus.Published)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Course is not published.");
            }

         
            if (course.Instructor.UserId != _currentUser.UserId)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to create assignments for this course.");
            }

         

            bool exists = await _unitOfWork.Assignments.Query()
                .AnyAsync(a =>
                    a.CourseId == request.CourseId &&
                    a.Title == request.Title &&
                    !a.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Assignment title already exists.");
            }

        

            Assignment assignment = new()
            {
                CourseId = request.CourseId,
                Title = request.Title,
                Description = request.Description,
                MaxScore = request.MaxScore,
                OpenAt = request.OpenAt,
                DueDate = request.DueDate,
                AllowLateSubmission = request.AllowLateSubmission,

                IsPublished = false,

                CreatedAt = DateTime.Now,
                CreatedBy = _currentUser.UserName
            };

            await _unitOfWork.Assignments.AddAsync(assignment);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(assignment.Id);
        }
    }
}
