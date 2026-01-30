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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(1000);

            builder.Property(x => x.IsVerified)
                .HasDefaultValue(false);

            builder.Property(x => x.IsApproved)
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(x => x.CarId)
                .HasDatabaseName("IX_Reviews_CarId");

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Reviews_UserId");

            builder.HasIndex(x => x.BookingId)
                .IsUnique() // Bir booking için sadece 1 review
                .HasDatabaseName("IX_Reviews_BookingId");

            builder.HasIndex(x => x.IsApproved)
                .HasDatabaseName("IX_Reviews_IsApproved");

            // Relationships
            builder.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Car)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
