using ElearningPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Persistence.Configuration
{
    public class CertificateConfiguration
       : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.CourseId)
                .IsRequired();

            builder.Property(x => x.CertificateNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.CertificateNumber)
                .IsUnique();

            builder.Property(x => x.VerificationCode)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.VerificationCode)
                .IsUnique();

            builder.Property(x => x.CertificateUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IssuedAt)
                .IsRequired();

            builder.Property(x => x.IsRevoked)
                .HasDefaultValue(false);

            builder.Property(x => x.RevokedReason)
                .HasMaxLength(500);

            builder.Property(x => x.DownloadCount)
                .HasDefaultValue(0);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Certificates)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Certificates)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

           
            builder.HasIndex(x => new
            {
                x.StudentId,
                x.CourseId
            }).IsUnique();
        }
    }
}
