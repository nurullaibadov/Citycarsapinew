using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Booking
{
    public class CreateBookingDto
    {
        public Guid CarId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Guid PickupLocationId { get; set; }
        public Guid ReturnLocationId { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
