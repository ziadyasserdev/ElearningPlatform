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
    public class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
    {
        public void Configure(EntityTypeBuilder<ExamAttempt> builder)
        {
            builder.ToTable("ExamAttempts");

            builder.Property(ea => ea.Score)
                .HasDefaultValue(0);

            builder.Property(ea => ea.Status)
                .HasConversion<int>();

            builder.Property(ea => ea.AttemptNumber)
                .HasDefaultValue(1);

            builder.HasQueryFilter(ea => !ea.IsDeleted);

            builder.HasOne(ea => ea.Student)
                .WithMany(u => u.ExamAttempts)
                .HasForeignKey(ea => ea.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ea => ea.Exam)
                .WithMany(ex => ex.ExamAttempts)
                .HasForeignKey(ea => ea.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
