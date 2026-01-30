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
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Country)
                .HasMaxLength(100)
                .HasDefaultValue("Azerbaijan");

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(x => x.WorkingHours)
                .HasMaxLength(200);

            // Latitude ve Longitude için decimal precision
            builder.Property(x => x.Latitude)
                .HasPrecision(9, 6); // 9 digit, 6 decimal (örnek: 40.123456)

            builder.Property(x => x.Longitude)
                .HasPrecision(9, 6);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.City)
                .HasDatabaseName("IX_Locations_City");

            // Relationships
            builder.HasMany(x => x.Cars)
                .WithOne(x => x.Location)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.SetNull); // Location silinince Car.LocationId = null

            // Pickup Location
            builder.HasMany(x => x.PickupLocationBookings)
                .WithOne(x => x.PickupLocation)
                .HasForeignKey(x => x.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Return Location
            builder.HasMany(x => x.ReturnLocationBookings)
                .WithOne(x => x.ReturnLocation)
                .HasForeignKey(x => x.ReturnLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
