using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Car
{
    public class CarListDto
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string? MainImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public double? AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
