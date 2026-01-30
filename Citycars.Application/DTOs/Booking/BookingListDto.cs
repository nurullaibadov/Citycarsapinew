using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Booking
{
    public class BookingListDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal FinalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string? CarImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
