using Citycars.Domain.Entities.Common;
using Citycars.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Booking : BaseEntity
    {
        /// <summary>
        /// Rezervasyon numarası
        /// Örnek: "BK-2024-00001"
        /// Format: BK-{Year}-{SequenceNumber}
        /// Unique olmalı
        /// Müşteriye gösterilir, takip için kullanılır
        /// </summary>
        public string BookingNumber { get; set; } = string.Empty;

        /// <summary>
        /// Teslim alma tarihi-saati
        /// UTC timezone
        /// Frontend'de kullanıcının timezone'una çevrilecek
        /// </summary>
        public DateTime PickupDate { get; set; }

        /// <summary>
        /// Teslim etme tarihi-saati
        /// PickupDate'den sonra olmalı (validation)
        /// </summary>
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// Toplam gün sayısı
        /// Hesaplama: (ReturnDate - PickupDate).Days
        /// Cache olarak saklanır
        /// Örnek: 3 gün
        /// </summary>
        public int TotalDays { get; set; }

        /// <summary>
        /// Rezervasyon sırasındaki günlük fiyat
        /// Araba fiyatı değişse bile bu sabit kalır
        /// Örnek: 150.00 AZN
        /// </summary>
        public decimal PricePerDay { get; set; }

        /// <summary>
        /// Toplam fiyat (indirim öncesi)
        /// Hesaplama: PricePerDay * TotalDays
        /// Örnek: 450.00 AZN (3 gün * 150 AZN)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// İndirim miktarı
        /// Nullable - her rezervasyonda indirim olmayabilir
        /// Örnek: 50.00 AZN (kupon kodu, sezon indirimi, vs.)
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Nihai fiyat (indirim sonrası)
        /// Hesaplama: TotalPrice - DiscountAmount
        /// Örnek: 400.00 AZN
        /// ÖDENİLECEK MİKTAR
        /// </summary>
        public decimal FinalPrice { get; set; }

        /// <summary>
        /// Rezervasyon durumu
        /// enum: Pending, Confirmed, InProgress, Completed, Cancelled
        /// İş akışı:
        /// Pending → Ödeme bekleniyor
        /// Confirmed → Ödeme yapıldı, onaylandı
        /// InProgress → Araç teslim alındı
        /// Completed → Araç teslim edildi
        /// Cancelled → İptal edildi
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Özel istekler
        /// Örnek: "Bebek koltuğu istiyorum", "Havalimanında karşılama"
        /// Textarea input
        /// </summary>
        public string? SpecialRequests { get; set; }

        /// <summary>
        /// İptal nedeni
        /// Sadece Status = Cancelled ise dolu
        /// Örnek: "Planım değişti", "Başka araba kiraladım"
        /// </summary>
        public string? CancellationReason { get; set; }

        /// <summary>
        /// İptal tarihi
        /// Sadece Status = Cancelled ise dolu
        /// İade politikası için önemli
        /// </summary>
        public DateTime? CancellationDate { get; set; }

        // ============================================
        // FOREIGN KEYS
        // ============================================

        /// <summary>
        /// Rezervasyonu yapan kullanıcı
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Kiralanan araba
        /// </summary>
        public Guid CarId { get; set; }

        /// <summary>
        /// Teslim alma noktası
        /// </summary>
        public Guid PickupLocationId { get; set; }

        /// <summary>
        /// Teslim etme noktası
        /// Farklı olabilir (örnek: Havalimanından al, ofise bırak)
        /// </summary>
        public Guid ReturnLocationId { get; set; }

        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================

        public User User { get; set; } = null!;
        public Car Car { get; set; } = null!;
        public Location PickupLocation { get; set; } = null!;
        public Location ReturnLocation { get; set; } = null!;

        /// <summary>
        /// Ödeme bilgisi
        /// One-to-One: 1 Booking → 1 Payment
        /// Nullable - henüz ödeme yapılmamış olabilir
        /// </summary>
        public Payment? Payment { get; set; }

        /// <summary>
        /// Yorum
        /// One-to-One: 1 Booking → 1 Review
        /// Nullable - tamamlanmış rezervasyonlarda yorum bırakılabilir
        /// </summary>
        public Review? Review { get; set; }
    }
}
