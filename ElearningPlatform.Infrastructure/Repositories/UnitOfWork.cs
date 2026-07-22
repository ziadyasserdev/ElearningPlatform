using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using ElearningPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Repositories
    
        public ICourseRepository Courses { get; private set; }
        public ISectionRepository Sections { get; private set; }
        public ILessonRepository Lessons { get; private set; }
        public IVideoRepository Videos { get; private set; }
     
        public IEnrollmentRepository Enrollments { get; private set; }
        public IAssignmentRepository Assignments { get; private set; }
        public ISubmissionRepository Submissions { get; private set; }
        public IExamRepository Exams { get; private set; }
        public IStudentAnswerRepository StudentAnswers { get; private set; }
        public IInstructorRepository Instructors { get; private set; }
        public ICertificateRepository Certificates { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IReviewRepository Reviews { get; private set; }

        public ICategoryRepository Categories {get;private set;}

        public IUserRepository Users { get; private set; }

        public IVideoProgressRepository VideoProgresses {  get; private set; }

        public IVideoCommentRepository videoComments { get; private set; }

        public IExamAttemptsRepository ExamAttempts { get; private set; }

        public IQuestionsRepository Questions { get; private set; }

     

        public IAssignmentAttachmentRepository AssignmentAttachments { get;private set; }

        public ICartRepository Carts { get; private set; }

        public ICartItemRepository CartItems { get; private set; }

        public ICouponRepository Coupons { get; private set; }

        public ICouponUsageRepository CouponUsages { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public IOrderItemRepository OrderItems { get; private set; }

        public IPaymentRepository Payments { get; private set; }

        public IAnswerRepository Answers { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Answers = new AnswerRepository(_context);
            Payments = new PaymentRepository(_context);
            Orders = new OrderRepository(_context);
            OrderItems = new OrderItemRepository(_context);
            Carts = new CartRepository(_context);
            Coupons = new CouponRepository(_context);
            CouponUsages = new CouponUsageRepository(_context);
            CartItems = new CartItemRepository(_context);
            AssignmentAttachments = new AssignmentAttachmentRepository(_context);
           
            Questions = new QuestionsRepository(_context);
            ExamAttempts = new ExamAttemptsRepository(_context);
            videoComments = new VideoCommentRepository(_context);
            VideoProgresses = new VideoProgressRepository(_context);
            // Initialize Repositories
            Categories = new CategoryRepository(_context);
                Users = new UserRepository(_context);
            Courses = new CourseRepository(_context);
            Sections = new SectionRepository(_context);
            Lessons = new LessonRepository(_context);
            Videos = new VideoRepository(_context);
         
            Enrollments = new EnrollmentRepository(_context);
            Assignments = new AssignmentRepository(_context);
            Submissions = new SubmissionRepository(_context);
            Exams = new ExamRepository(_context);
            StudentAnswers = new StudentAnswerRepository(_context);
            Instructors = new InstructorRepository(_context);
            Certificates = new CertificateRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Reviews = new ReviewRepository(_context);
        }

        // Transaction support
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        // Save Changes
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose pattern
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
