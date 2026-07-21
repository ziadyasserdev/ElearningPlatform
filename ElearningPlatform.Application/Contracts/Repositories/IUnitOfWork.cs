using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Users
    

        // Courses & Structure
        ICourseRepository Courses { get; }
        ISectionRepository Sections { get; }
        ILessonRepository Lessons { get; }
        IVideoRepository Videos { get; }
        ICategoryRepository Categories { get; }
        // Student Progress & Enrollment
       IVideoProgressRepository VideoProgresses { get; }
        IEnrollmentRepository Enrollments { get; }
        IQuestionsRepository Questions { get; }
        IVideoCommentRepository videoComments { get; }
        IAnswerRepository Answers { get; }
        IExamAttemptsRepository ExamAttempts { get; }
        // Assignments & Exams
        IAssignmentRepository Assignments { get; }
        ISubmissionRepository Submissions { get; }
        IExamRepository Exams { get; }
        IUserRepository Users { get; }
        IStudentAnswerRepository StudentAnswers { get; }
        IInstructorRepository Instructors { get; }
        IAssignmentAttachmentRepository AssignmentAttachments { get; }
        ICouponRepository Coupons { get; }
        ICouponUsageRepository CouponUsages { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        // Certificates & Attendance
        ICertificateRepository Certificates { get; }
        IAttendanceRepository Attendances { get; }

        // Reviews
        IReviewRepository Reviews { get; }

        // Transactions
        Task<IDbContextTransaction> BeginTransactionAsync();

        // Save Changes
        Task<int> SaveAsync();
    }
}
