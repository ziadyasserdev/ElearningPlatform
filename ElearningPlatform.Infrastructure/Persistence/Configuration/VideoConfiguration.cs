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
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Videos");

            builder.Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(v => v.ThumbnailUrl)
                .HasMaxLength(500);

            builder.Property(v => v.Duration)
                .HasDefaultValue(0);

            builder.Property(v => v.FileSize)
                .HasDefaultValue(0);

            builder.Property(v => v.Format)
                .HasMaxLength(50);

            builder.Property(v => v.ProcessingStatus)
                .HasConversion<int>();

          

            builder.HasOne(v => v.Uploader)
                .WithMany()
                .HasForeignKey(v => v.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
