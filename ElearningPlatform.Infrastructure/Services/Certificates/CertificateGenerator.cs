using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Services.Certificates
{
    public class CertificateGenerator : ICertificateGenerator
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment environment;

        public CertificateGenerator(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment)
        {
            this.unitOfWork = unitOfWork;
            this.environment = environment;
        }

        public async Task<string> GenerateAsync(
            Certificate certificate,
            CancellationToken cancellationToken = default)
        {
            var entity = await unitOfWork.Certificates.Query()
                .Include(x => x.Student)
                .Include(x => x.Course)
                    .ThenInclude(x => x.Instructor)
                        .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == certificate.Id,
                    cancellationToken);

            if (entity == null)
                throw new Exception("Certificate not found.");

            var model = new CertificateDocumentModel
            {
                StudentName = entity.Student.FullName,
                CourseTitle = entity.Course.Title,
                InstructorName = entity.Course.Instructor.User.FullName,
                CertificateNumber = entity.CertificateNumber,
                VerificationCode = entity.VerificationCode,
                IssuedAt = entity.IssuedAt
            };

            var year = entity.IssuedAt.Year;

            var folder = Path.Combine(
                environment.WebRootPath,
                "certificates",
                year.ToString());

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = $"{entity.CertificateNumber}.pdf";

            var fullPath = Path.Combine(folder, fileName);

            var document = new CertificateDocument(model);

            document.GeneratePdf(fullPath);

            return $"/certificates/{year}/{fileName}";
        }
    }
}
