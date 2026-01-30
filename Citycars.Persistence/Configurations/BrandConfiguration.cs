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
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LogoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.Name)
                .IsUnique()
                .HasDatabaseName("IX_Brands_Name");

            // Relationships
            builder.HasMany(x => x.Cars)
                .WithOne(x => x.Brand)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
