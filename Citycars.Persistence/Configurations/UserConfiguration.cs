using Citycars.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // ============================================
            // TABLO ADI
            // ============================================
            builder.ToTable("Users");

            // ============================================
            // PRIMARY KEY
            // ============================================
            builder.HasKey(x => x.Id);

            // ============================================
            // PROPERTIES
            // ============================================

            // FirstName
            builder.Property(x => x.FirstName)
                .IsRequired() // NOT NULL
                .HasMaxLength(50); // VARCHAR(50)

            // LastName
            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // Email
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            // PasswordHash
            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500); // BCrypt hash uzun olabilir

            // PhoneNumber (Nullable)
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20);

            // DrivingLicenseNumber (Nullable)
            builder.Property(x => x.DrivingLicenseNumber)
                .HasMaxLength(50);

            // Address (Nullable)
            builder.Property(x => x.Address)
                .HasMaxLength(500);

            // City (Nullable)
            builder.Property(x => x.City)
                .HasMaxLength(100);

            // Country (Nullable)
            builder.Property(x => x.Country)
                .HasMaxLength(100);

            // Role
            builder.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("User"); // Default değer

            // IsEmailVerified
            builder.Property(x => x.IsEmailVerified)
                .HasDefaultValue(false);

            // IsActive
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // ============================================
            // INDEXES
            // ============================================

            // Email unique olmalı (aynı email ile 2 kayıt olamaz)
            builder.HasIndex(x => x.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            // Email ile arama hızlı olsun
            builder.HasIndex(x => x.Email)
                .HasDatabaseName("IX_Users_Email_Lookup");

            // PhoneNumber ile arama
            builder.HasIndex(x => x.PhoneNumber)
                .HasDatabaseName("IX_Users_PhoneNumber");

            // ============================================
            // RELATIONSHIPS
            // ============================================

            // User -> Bookings (One-to-Many)
            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict); // User silinince Booking'ler silinmesin

            // User -> Reviews (One-to-Many)
            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
