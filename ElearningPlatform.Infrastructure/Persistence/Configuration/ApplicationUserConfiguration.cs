using ElearningPlatform.Domain.Identity;
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
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.ProfileImageUrl)
                .HasMaxLength(500);

          

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

        

            builder.HasOne(u => u.InstructorProfile)
                .WithOne(ip => ip.User)
                .HasForeignKey<InstructorProfile>(ip => ip.UserId)
                .OnDelete(DeleteBehavior.Cascade);
         
        }
    }
}
