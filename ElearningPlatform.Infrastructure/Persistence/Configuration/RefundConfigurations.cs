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
    public class RefundConfigurations : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.HasOne(x => x.Payment)
                .WithOne(x => x.Refund)
                .HasForeignKey<Refund>(x => x.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
