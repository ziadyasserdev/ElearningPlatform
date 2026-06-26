using ElearningPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Persistence.Configuration
{
    public class VideoProgressConfiguration : IEntityTypeConfiguration<VideoProgress>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<VideoProgress> builder)
        {
            builder.HasKey(x => new { x.UserId, x.VideoId });

            builder.Property(x => x.WatchedSeconds).IsRequired();
            builder.Property(x => x.IsCompleted).HasDefaultValue(false);

            builder.Property(x => x.LastWatchedAt).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(u => u.VideoProgresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Video)
                .WithMany(v => v.VideoProgresses)
                .HasForeignKey(x => x.VideoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
