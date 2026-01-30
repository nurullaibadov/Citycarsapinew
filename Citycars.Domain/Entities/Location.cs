using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Location : BaseEntity
    {
        /// <summary>
        /// Lokasyon adı
        /// Örnek: "Heydar Aliyev Airport", "City Center Office"
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Tam adres
        /// Örnek: "Heydar Aliyev International Airport, Baku"
        /// Google Maps için
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Şehir
        /// Örnek: "Baku", "Ganja"
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Ülke
        /// Default: Azerbaijan
        /// İleride international expansion için
        /// </summary>
        public string? Country { get; set; } = "Azerbaijan";

        /// <summary>
        /// Enlem (Latitude)
        /// Google Maps koordinatı
        /// Örnek: 40.4673
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Boylam (Longitude)
        /// Google Maps koordinatı
        /// Örnek: 50.0472
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// İletişim telefonu
        /// Örnek: "+994 12 555 55 55"
        /// Her lokasyonun kendi numarası olabilir
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Çalışma saatleri
        /// Örnek: "24/7" veya "09:00 - 18:00"
        /// JSON olarak da saklanabilir
        /// </summary>
        public string? WorkingHours { get; set; }

        /// <summary>
        /// Lokasyon aktif mi?
        /// Kapalı/bakımda olan lokasyonlar
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================

        /// <summary>
        /// Bu lokasyonda bulunan arabalar
        /// One-to-Many: 1 Location → Birden fazla Car
        /// </summary>
        public ICollection<Car> Cars { get; set; } = new List<Car>();

        /// <summary>
        /// Bu lokasyondan alınan rezervasyonlar
        /// One-to-Many: 1 PickupLocation → Birden fazla Booking
        /// </summary>
        public ICollection<Booking> PickupLocationBookings { get; set; } = new List<Booking>();

        /// <summary>
        /// Bu lokasyona bırakılan rezervasyonlar
        /// One-to-Many: 1 ReturnLocation → Birden fazla Booking
        /// </summary>
        public ICollection<Booking> ReturnLocationBookings { get; set; } = new List<Booking>();
    }
}
