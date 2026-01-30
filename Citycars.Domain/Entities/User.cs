using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class User : BaseEntity
    {
        /// <summary>
        /// Kullanıcının adı
        /// string.Empty ile initialize et (null reference hatalarını önlemek için)
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının soyadı
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email adresi
        /// - Unique olmalı (Database constraint ile)
        /// - Login için kullanılacak
        /// - Lowercase'e dönüştürülecek
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre HASH'i
        /// ASLA plain text şifre saklanmaz!
        /// BCrypt veya PBKDF2 ile hash'lenecek
        /// Örnek: $2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Telefon numarası
        /// Nullable - zorunlu değil
        /// Format: +994501234567
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Sürücü belgesi numarası
        /// Araç kiralamak için gerekli
        /// Nullable - kayıt sırasında olmayabilir
        /// </summary>
        public string? DrivingLicenseNumber { get; set; }

        /// <summary>
        /// Doğum tarihi
        /// - 18 yaş kontrolü için
        /// - Nullable - opsiyonel
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Adres bilgisi
        /// Nullable - opsiyonel
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Şehir
        /// Default: Baku
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Ülke
        /// Default: Azerbaijan
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Email doğrulandı mı?
        /// Email'e gönderilen link ile doğrulanacak
        /// false ise bazı işlemlere izin verilmeyebilir
        /// </summary>
        public bool IsEmailVerified { get; set; } = false;

        /// <summary>
        /// Hesap aktif mi?
        /// Admin tarafından ban edilebilir (IsActive = false)
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Kullanıcı rolü
        /// - "User": Normal müşteri
        /// - "Admin": Yönetici
        /// Default: User
        /// İleride enum'a çevrilebilir
        /// </summary>
        public string Role { get; set; } = "User";

        // ============================================
        // NAVIGATION PROPERTIES (İlişkiler)
        // ============================================

        /// <summary>
        /// Kullanıcının yaptığı rezervasyonlar
        /// One-to-Many ilişki: 1 User → Birden fazla Booking
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        /// <summary>
        /// Kullanıcının yazdığı yorumlar
        /// One-to-Many ilişki: 1 User → Birden fazla Review
        /// </summary>
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
