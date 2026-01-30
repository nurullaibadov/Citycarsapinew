using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Booking
{
    public class CarBookingDto
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string? MainImageUrl { get; set; }
        public int Year { get; set; }
    }
}
