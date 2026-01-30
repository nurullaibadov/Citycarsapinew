using Citycars.Application.DTOs.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Booking
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalDays { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? SpecialRequests { get; set; }

        // Navigation properties
        public UserBookingDto User { get; set; } = null!;
        public CarBookingDto Car { get; set; } = null!;
        public LocationDto PickupLocation { get; set; } = null!;
        public LocationDto ReturnLocation { get; set; } = null!;
        public PaymentDto? Payment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
