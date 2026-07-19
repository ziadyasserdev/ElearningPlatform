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
    public class AssignmentAttachmentConfiguration
     : IEntityTypeConfiguration<AssignmentAttachment>
    {
        public void Configure(EntityTypeBuilder<AssignmentAttachment> builder)
        {
            builder.ToTable("AssignmentAttachments");

            builder.Property(x => x.FileName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.FileUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.ContentType)
                .HasMaxLength(100);

            builder.HasOne(x => x.Assignment)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.AssignmentId);
        }
    }
}
