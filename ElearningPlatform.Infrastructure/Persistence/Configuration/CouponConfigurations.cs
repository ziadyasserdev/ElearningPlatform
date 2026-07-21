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
    public class CouponConfigurations : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.Value)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.MinimumOrderAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.MaximumDiscountAmount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
