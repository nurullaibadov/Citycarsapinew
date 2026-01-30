using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Car
{
    public class CreateCarDto
    {
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public int Seats { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public string Transmission { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public decimal? PricePerHour { get; set; }
        public int Mileage { get; set; }
        public string? Description { get; set; }
        public List<string> Features { get; set; } = new();
        public string Status { get; set; } = "Available";
        public string? MainImageUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public bool IsFeatured { get; set; }

        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Guid? LocationId { get; set; }
    }
}
