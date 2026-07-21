using DocumentFormat.OpenXml.Drawing;
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
    public class PaymentConfigurations : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Currency)
                .HasMaxLength(10);

            builder.Property(x => x.TransactionId)
                .HasMaxLength(200);

            builder.Property(x => x.PaymentIntentId)
                .HasMaxLength(200);

            builder.Property(x => x.FailureReason)
                .HasMaxLength(500);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Payment)
                .HasForeignKey<Payment>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Refund)
                .WithOne(x => x.Payment)
                .HasForeignKey<Refund>(x => x.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
