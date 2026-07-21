using ClosedXML.Excel;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.ExportSubmissions
{
    public class ExportSubmissionsQueryHandler
     : IRequestHandler<
         ExportSubmissionsQuery,
         Result<ExportSubmissionsResult>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ExportSubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ExportSubmissionsResult>> Handle(
            ExportSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<ExportSubmissionsResult>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            // Verify that the assignment belongs
            // to the current instructor
            var assignment = await unitOfWork.Assignments
                .Query()
                .AsNoTracking()
                .Where(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted &&
                    a.Course.Instructor.UserId == userId)
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    CourseTitle = a.Course.Title
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (assignment is null)
            {
                return Result<ExportSubmissionsResult>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            var submissions = await unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted)
                .Select(s => new
                {
                    StudentName = s.Student.FullName,

                    StudentEmail = s.Student.Email,

                    s.SubmittedAt,

                    s.IsLate,

                    s.Score,

                    MaxScore = s.Assignment.MaxScore,

                    s.Feedback,

                    s.GradedAt,

                    s.Status
                })
                .OrderBy(s => s.StudentName)
                .ToListAsync(cancellationToken);

            if (submissions.Count == 0)
            {
                return Result<ExportSubmissionsResult>.Failure(
                    ResultStatus.NotFound,
                    "No submissions found for this assignment.");
            }

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add(
                "Submissions");

         
            worksheet.Cell(1, 1).Value =
                assignment.Title;

            worksheet.Cell(1, 1)
                .Style.Font.Bold = true;

            worksheet.Cell(1, 1)
                .Style.Font.FontSize = 16;

          
            worksheet.Cell(2, 1).Value =
                "Course";

            worksheet.Cell(2, 2).Value =
                assignment.CourseTitle;

         
            var headerRow = 4;

            worksheet.Cell(headerRow, 1)
                .Value = "Student Name";

            worksheet.Cell(headerRow, 2)
                .Value = "Student Email";

            worksheet.Cell(headerRow, 3)
                .Value = "Submitted At";

            worksheet.Cell(headerRow, 4)
                .Value = "Is Late";

            worksheet.Cell(headerRow, 5)
                .Value = "Score";

            worksheet.Cell(headerRow, 6)
                .Value = "Max Score";

            worksheet.Cell(headerRow, 7)
                .Value = "Feedback";

            worksheet.Cell(headerRow, 8)
                .Value = "Graded At";

            worksheet.Cell(headerRow, 9)
                .Value = "Status";

          
            var headerRange = worksheet.Range(
                headerRow,
                1,
                headerRow,
                9);

            headerRange.Style.Font.Bold = true;

           
            var currentRow = headerRow + 1;

            foreach (var submission in submissions)
            {
                worksheet.Cell(currentRow, 1)
                    .Value = submission.StudentName;

                worksheet.Cell(currentRow, 2)
                    .Value = submission.StudentEmail;

                worksheet.Cell(currentRow, 3)
                    .Value = submission.SubmittedAt;

                worksheet.Cell(currentRow, 4)
                    .Value = submission.IsLate;

                worksheet.Cell(currentRow, 5)
                    .Value = submission.Score;

                worksheet.Cell(currentRow, 6)
                    .Value = submission.MaxScore;

                worksheet.Cell(currentRow, 7)
                    .Value = submission.Feedback;

                worksheet.Cell(currentRow, 8)
                    .Value = submission.GradedAt;

                worksheet.Cell(currentRow, 9)
                    .Value = submission.Status.ToString();

                currentRow++;
            }

          
            worksheet.Column(3)
                .Style.DateFormat.Format =
                "yyyy-MM-dd HH:mm";

            worksheet.Column(8)
                .Style.DateFormat.Format =
                "yyyy-MM-dd HH:mm";

         
            var dataRange = worksheet.Range(
                headerRow,
                1,
                currentRow - 1,
                9);

            dataRange.CreateTable();

          
            worksheet.Columns()
                .AdjustToContents();

            using var memoryStream = new MemoryStream();

            workbook.SaveAs(memoryStream);

            return Result<ExportSubmissionsResult>.Success(
                new ExportSubmissionsResult
                {
                    FileBytes =
                        memoryStream.ToArray(),

                    FileName =
                        $"{assignment.Title}_Submissions.xlsx",

                    ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                });
        }
    }
}
