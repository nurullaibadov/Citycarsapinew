using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Payment : BaseEntity
    {
        /// <summary>
        /// Transaction ID
        /// Ödeme gateway'inden dönen benzersiz ID
        /// Örnek: "TXN-2024-ABC123"
        /// Ödeme sorgulama için kullanılır
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// Ödeme miktarı
        /// Booking.FinalPrice ile aynı olmalı
        /// decimal kullan (para için)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Para birimi
        /// Default: AZN (Azerbaycan Manatı)
        /// İleride multi-currency: USD, EUR, TRY
        /// </summary>
        public string Currency { get; set; } = "AZN";

        /// <summary>
        /// Ödeme durumu
        /// enum: Pending, Processing, Completed, Failed, Refunded
        /// </summary>
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// Ödeme yöntemi
        /// Örnek: "Card", "Cash", "Bank Transfer"
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// Kart son 4 hanesi
        /// Güvenlik: Sadece son 4 hane saklanır
        /// Örnek: "4321"
        /// Tam kart numarası ASLA saklanmaz (PCI-DSS compliance)
        /// </summary>
        public string? CardLast4Digits { get; set; }

        /// <summary>
        /// Ödeme tarihi
        /// Ödeme başarılı olduğunda set edilir
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Payment Gateway'den dönen response (JSON)
        /// Debug ve sorun çözme için
        /// Örnek: {"status": "success", "authCode": "123456"}
        /// Production'da encrypt edilebilir
        /// </summary>
        public string? PaymentGatewayResponse { get; set; }

        // ============================================
        // FOREIGN KEY
        // ============================================

        /// <summary>
        /// İlgili rezervasyon
        /// One-to-One ilişki
        /// </summary>
        public Guid BookingId { get; set; }

        // ============================================
        // NAVIGATION PROPERTY
        // ============================================

        public Booking Booking { get; set; } = null!;
    }
}
