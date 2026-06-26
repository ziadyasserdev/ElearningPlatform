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
    public class InstructorProfileConfiguration : IEntityTypeConfiguration<InstructorProfile>
    {
        public void Configure(EntityTypeBuilder<InstructorProfile> builder)
        {
            builder.ToTable("InstructorProfiles");

            builder.Property(ip => ip.Specialization)
                .HasMaxLength(200);

            builder.Property(ip => ip.ExperienceYears)
                .HasDefaultValue(0);

            builder.Property(ip => ip.Rating)
                .HasPrecision(3, 2)
                .HasDefaultValue(0);

           
        }
    }
}
