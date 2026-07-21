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

    public class CouponUsageConfigurations : IEntityTypeConfiguration<CouponUsage>
    {
        public void Configure(EntityTypeBuilder<CouponUsage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.UsedAt)
                .IsRequired();

            builder.HasOne(x => x.Coupon)
                .WithMany(x => x.CouponUsages)
                .HasForeignKey(x => x.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.CouponId,
                x.StudentId,
                x.OrderId
            }).IsUnique();
        }
    }
}
