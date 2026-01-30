using Citycars.Domain.Entities;
using Citycars.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TransactionId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasDefaultValue("AZN");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(PaymentStatus.Pending);

            builder.Property(x => x.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.CardLast4Digits)
                .HasMaxLength(4);

            builder.Property(x => x.PaymentGatewayResponse)
                .HasColumnType("nvarchar(max)"); // JSON

            // Indexes
            builder.HasIndex(x => x.TransactionId)
                .IsUnique()
                .HasDatabaseName("IX_Payments_TransactionId");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Payments_Status");

            builder.HasIndex(x => x.BookingId)
                .IsUnique() // One-to-One ilişki
                .HasDatabaseName("IX_Payments_BookingId");
        }
    }
}
