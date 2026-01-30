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
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");
            builder.HasKey(x => x.Id);

            // ============================================
            // PROPERTIES
            // ============================================

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Year)
                .IsRequired();

            builder.Property(x => x.Color)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Seats)
                .IsRequired();

            builder.Property(x => x.FuelType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Transmission)
                .IsRequired()
                .HasMaxLength(50);

            // Para için decimal precision
            builder.Property(x => x.PricePerDay)
                .IsRequired()
                .HasPrecision(18, 2) // 18 digit, 2 decimal (örnek: 9999999999999999.99)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PricePerHour)
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Mileage)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // JSON field
            builder.Property(x => x.Features)
                .HasColumnType("nvarchar(max)"); // JSON için

            // Enum as string (database'de "Available", "Booked" olarak saklanır)
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>() // Enum → String
                .HasMaxLength(50)
                .HasDefaultValue(CarStatus.Available);

            builder.Property(x => x.MainImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.ImageUrls)
                .HasColumnType("nvarchar(max)"); // JSON array

            builder.Property(x => x.IsFeatured)
                .HasDefaultValue(false);

            builder.Property(x => x.AverageRating)
                .HasPrecision(3, 2); // 5.00 formatında

            builder.Property(x => x.TotalReviews)
                .HasDefaultValue(0);

            // ============================================
            // INDEXES
            // ============================================

            // LicensePlate unique
            builder.HasIndex(x => x.LicensePlate)
                .IsUnique()
                .HasDatabaseName("IX_Cars_LicensePlate");

            // Status ile filtreleme (müsait araçlar)
            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Cars_Status");

            // IsFeatured ile filtreleme (öne çıkan araçlar)
            builder.HasIndex(x => x.IsFeatured)
                .HasDatabaseName("IX_Cars_IsFeatured");

            // CategoryId ile filtreleme
            builder.HasIndex(x => x.CategoryId)
                .HasDatabaseName("IX_Cars_CategoryId");

            // BrandId ile filtreleme
            builder.HasIndex(x => x.BrandId)
                .HasDatabaseName("IX_Cars_BrandId");

            // Composite index (kategori + durum)
            builder.HasIndex(x => new { x.CategoryId, x.Status })
                .HasDatabaseName("IX_Cars_Category_Status");

            // ============================================
            // RELATIONSHIPS
            // ============================================

            // Car -> Category (Many-to-One)
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car -> Brand (Many-to-One)
            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car -> Location (Many-to-One, Nullable)
            builder.HasOne(x => x.Location)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Car -> Bookings (One-to-Many)
            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car -> Reviews (One-to-Many)
            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade); // Car silinince review'ler de silinsin
        }
    }
}
