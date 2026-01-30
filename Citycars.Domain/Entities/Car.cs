using Citycars.Domain.Entities.Common;
using Citycars.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Car : BaseEntity
    {
        /// <summary>
        /// Araç modeli
        /// Örnek: "S-Class", "X5", "Model 3"
        /// Brand ile birlikte: "Mercedes S-Class"
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Üretim yılı
        /// Örnek: 2024
        /// Min: 2000, Max: current year + 1
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Renk
        /// Örnek: "Black", "White", "Silver"
        /// Frontend'de filter için
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Plaka numarası
        /// Örnek: "90-AA-123"
        /// Unique olmalı
        /// Admin panelde otomatik generate edilebilir
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Koltuk sayısı
        /// Örnek: 5, 7, 2 (spor araba)
        /// Filter için önemli
        /// </summary>
        public int Seats { get; set; }

        /// <summary>
        /// Yakıt tipi
        /// Örnek: "Petrol", "Diesel", "Electric", "Hybrid"
        /// İleride enum olabilir
        /// </summary>
        public string FuelType { get; set; } = string.Empty;

        /// <summary>
        /// Vites tipi
        /// Örnek: "Automatic", "Manual"
        /// enum yapılabilir
        /// </summary>
        public string Transmission { get; set; } = string.Empty;

        /// <summary>
        /// Günlük kiralama ücreti
        /// Örnek: 150.00 AZN
        /// decimal kullan (para için float ASLA!)
        /// </summary>
        public decimal PricePerDay { get; set; }

        /// <summary>
        /// Saatlik kiralama ücreti
        /// Nullable - tüm araçlar saatlik kiralanmayabilir
        /// Örnek: 25.00 AZN/saat
        /// </summary>
        public decimal? PricePerHour { get; set; }

        /// <summary>
        /// Kilometre
        /// Örnek: 15000
        /// Araç durumu için önemli
        /// Her rezervasyon sonrası güncellenebilir
        /// </summary>
        public int Mileage { get; set; }

        /// <summary>
        /// Araç açıklaması
        /// Örnek: "2024 model Mercedes S-Class, tam donanımlı..."
        /// SEO için önemli
        /// Rich text olabilir
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Özellikler (JSON Array)
        /// Örnek: ["GPS", "Bluetooth", "Sunroof", "Leather Seats", "Parking Sensors"]
        /// JSON olarak sakla, deserialize et
        /// Frontend'de checkbox olarak göster
        /// </summary>
        public string? Features { get; set; }

        /// <summary>
        /// Araç durumu
        /// enum: Available, Booked, InMaintenance, OutOfService
        /// </summary>
        public CarStatus Status { get; set; } = CarStatus.Available;

        /// <summary>
        /// Ana görsel
        /// Örnek: "https://cdn.citycars.az/cars/mercedes-s-class-2024.jpg"
        /// Liste ve kart görünümünde gösterilir
        /// </summary>
        public string? MainImageUrl { get; set; }

        /// <summary>
        /// Diğer görseller (JSON Array)
        /// Örnek: ["url1.jpg", "url2.jpg", "url3.jpg"]
        /// Detay sayfasında galeri olarak gösterilir
        /// </summary>
        public string? ImageUrls { get; set; }

        /// <summary>
        /// Öne çıkan araç mı?
        /// Homepage'de ve kategori üstünde gösterilir
        /// Admin panelde checkbox
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Ortalama puan
        /// Örnek: 4.7
        /// Review'lerden hesaplanır
        /// Nullable - henüz review yoksa
        /// </summary>
        public double? AverageRating { get; set; }

        /// <summary>
        /// Toplam yorum sayısı
        /// Cache olarak saklanır (her seferinde count yapmamak için)
        /// Review eklenince/silinince güncellenir
        /// </summary>
        public int TotalReviews { get; set; } = 0;

        // ============================================
        // FOREIGN KEYS
        // ============================================

        /// <summary>
        /// Kategori ID
        /// Hangi kategoriye ait? (Luxury, SUV, vs.)
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Marka ID
        /// Hangi marka? (Mercedes, BMW, vs.)
        /// </summary>
        public Guid BrandId { get; set; }

        /// <summary>
        /// Lokasyon ID
        /// Araç şu anda nerede?
        /// Nullable - henüz atanmamış olabilir
        /// </summary>
        public Guid? LocationId { get; set; }

        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================

        /// <summary>
        /// Kategori navigation
        /// EF Core otomatik doldurur
        /// </summary>
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Marka navigation
        /// </summary>
        public Brand Brand { get; set; } = null!;

        /// <summary>
        /// Lokasyon navigation
        /// Nullable olabilir
        /// </summary>
        public Location? Location { get; set; }

        /// <summary>
        /// Rezervasyonlar
        /// Bu arabanın geçmiş ve gelecek rezervasyonları
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        /// <summary>
        /// Yorumlar
        /// Bu araba hakkında yazılan yorumlar
        /// </summary>
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
