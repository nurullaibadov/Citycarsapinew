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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(x => x.Id);

            // ============================================
            // PROPERTIES
            // ============================================

            builder.Property(x => x.BookingNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PickupDate)
                .IsRequired();

            builder.Property(x => x.ReturnDate)
                .IsRequired();

            builder.Property(x => x.TotalDays)
                .IsRequired();

            // Para alanları
            builder.Property(x => x.PricePerDay)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalPrice)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DiscountAmount)
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.FinalPrice)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            // Enum as string
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(BookingStatus.Pending);

            builder.Property(x => x.SpecialRequests)
                .HasMaxLength(1000);

            builder.Property(x => x.CancellationReason)
                .HasMaxLength(500);

            // ============================================
            // INDEXES
            // ============================================

            // BookingNumber unique
            builder.HasIndex(x => x.BookingNumber)
                .IsUnique()
                .HasDatabaseName("IX_Bookings_BookingNumber");

            // Status ile filtreleme
            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Bookings_Status");

            // UserId ile arama (kullanıcının rezervasyonları)
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Bookings_UserId");

            // CarId ile arama (arabanın rezervasyonları)
            builder.HasIndex(x => x.CarId)
                .HasDatabaseName("IX_Bookings_CarId");

            // Tarih aralığı aramaları için
            builder.HasIndex(x => new { x.PickupDate, x.ReturnDate })
                .HasDatabaseName("IX_Bookings_Dates");

            // ============================================
            // RELATIONSHIPS
            // ============================================

            // Booking -> User
            builder.HasOne(x => x.User)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Car
            builder.HasOne(x => x.Car)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> PickupLocation
            builder.HasOne(x => x.PickupLocation)
                .WithMany(x => x.PickupLocationBookings)
                .HasForeignKey(x => x.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> ReturnLocation
            builder.HasOne(x => x.ReturnLocation)
                .WithMany(x => x.ReturnLocationBookings)
                .HasForeignKey(x => x.ReturnLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Payment (One-to-One)
            builder.HasOne(x => x.Payment)
                .WithOne(x => x.Booking)
                .HasForeignKey<Payment>(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // Booking silinince Payment de silinsin

            // Booking -> Review (One-to-One)
            builder.HasOne(x => x.Review)
                .WithOne(x => x.Booking)
                .HasForeignKey<Review>(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
