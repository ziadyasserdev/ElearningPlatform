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
    public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.ToTable("StudentAnswers");

            builder.Property(sa => sa.TextAnswer)
                .HasMaxLength(2000);

            builder.Property(sa => sa.ScoreAwarded)
                .HasDefaultValue(0);

            builder.Property(sa => sa.AnsweredAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasQueryFilter(sa => !sa.IsDeleted);

            builder.HasOne(sa => sa.ExamAttempt)
                .WithMany(ea => ea.StudentAnswers)
                .HasForeignKey(sa => sa.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sa => sa.SelectedAnswer)
                .WithMany()
                .HasForeignKey(sa => sa.SelectedAnswerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
