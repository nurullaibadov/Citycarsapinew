using Citycars.Domain.Entities;
using Citycars.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence.Seeds
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // ============================================
            // CATEGORIES
            // ============================================

            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Luxury Cars",
                    Description = "Premium luxury vehicles for special occasions",
                    ImageUrl = "/images/categories/luxury.jpg",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "SUVs",
                    Description = "Spacious and comfortable SUVs for family trips",
                    ImageUrl = "/images/categories/suv.jpg",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Economy",
                    Description = "Affordable and fuel-efficient cars",
                    ImageUrl = "/images/categories/economy.jpg",
                    DisplayOrder = 3,
                    IsActive = true
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Electric",
                    Description = "Eco-friendly electric vehicles",
                    ImageUrl = "/images/categories/electric.jpg",
                    DisplayOrder = 4,
                    IsActive = true
                }
            };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // ============================================
            // BRANDS
            // ============================================

            if (!await context.Brands.AnyAsync())
            {
                var brands = new List<Brand>
            {
                new Brand { Id = Guid.NewGuid(), Name = "Mercedes-Benz", LogoUrl = "/images/brands/mercedes.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "BMW", LogoUrl = "/images/brands/bmw.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "Audi", LogoUrl = "/images/brands/audi.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "Tesla", LogoUrl = "/images/brands/tesla.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "Toyota", LogoUrl = "/images/brands/toyota.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "Hyundai", LogoUrl = "/images/brands/hyundai.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "Kia", LogoUrl = "/images/brands/kia.svg", IsActive = true },
                new Brand { Id = Guid.NewGuid(), Name = "Range Rover", LogoUrl = "/images/brands/range-rover.svg", IsActive = true }
            };

                await context.Brands.AddRangeAsync(brands);
                await context.SaveChangesAsync();
            }

            // ============================================
            // LOCATIONS
            // ============================================

            if (!await context.Locations.AnyAsync())
            {
                var locations = new List<Location>
            {
                new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "Heydar Aliyev International Airport",
                    Address = "Baku Airport, AZ1044",
                    City = "Baku",
                    Country = "Azerbaijan",
                    Latitude = 40.4675m,
                    Longitude = 50.0467m,
                    PhoneNumber = "+994 12 497 27 27",
                    WorkingHours = "24/7",
                    IsActive = true
                },
                new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "City Center Office",
                    Address = "28 May Street, Baku",
                    City = "Baku",
                    Country = "Azerbaijan",
                    Latitude = 40.3777m,
                    Longitude = 49.8920m,
                    PhoneNumber = "+994 12 555 55 55",
                    WorkingHours = "09:00 - 18:00",
                    IsActive = true
                },
                new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "Port Baku Mall",
                    Address = "153 Neftchilar Avenue, Baku",
                    City = "Baku",
                    Country = "Azerbaijan",
                    Latitude = 40.3656m,
                    Longitude = 49.8364m,
                    PhoneNumber = "+994 12 444 44 44",
                    WorkingHours = "10:00 - 22:00",
                    IsActive = true
                }
            };

                await context.Locations.AddRangeAsync(locations);
                await context.SaveChangesAsync();
            }

            // ============================================
            // ADMIN USER
            // ============================================

            if (!await context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@citycars.az",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), // Hash şifreyi
                    PhoneNumber = "+994501234567",
                    Role = "Admin",
                    IsEmailVerified = true,
                    IsActive = true,
                    City = "Baku",
                    Country = "Azerbaijan"
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }

            // ============================================
            // SETTINGS
            // ============================================

            if (!await context.Settings.AnyAsync())
            {
                var settings = new List<Setting>
            {
                new Setting { Id = Guid.NewGuid(), Key = "SiteName", Value = "City Cars AZ", Type = "String" },
                new Setting { Id = Guid.NewGuid(), Key = "ContactEmail", Value = "info@citycars.az", Type = "String" },
                new Setting { Id = Guid.NewGuid(), Key = "ContactPhone", Value = "+994 12 555 55 55", Type = "String" },
                new Setting { Id = Guid.NewGuid(), Key = "MaxBookingDays", Value = "30", Type = "Number" },
                new Setting { Id = Guid.NewGuid(), Key = "MinBookingHours", Value = "24", Type = "Number" },
                new Setting { Id = Guid.NewGuid(), Key = "CancellationDeadlineHours", Value = "24", Type = "Number" }
            };

                await context.Settings.AddRangeAsync(settings);
                await context.SaveChangesAsync();
            }
        }
    }
}
