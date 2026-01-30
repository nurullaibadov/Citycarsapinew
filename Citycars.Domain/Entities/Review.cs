using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Review : BaseEntity
    {
        /// <summary>
        /// Puan
        /// 1-5 arası (1: Çok kötü, 5: Mükemmel)
        /// Validation: Min=1, Max=5
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Yorum metni
        /// Nullable - sadece puan bırakılabilir
        /// Max length: 1000 karakter
        /// HTML encode edilecek (XSS saldırılarını önlemek için)
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Doğrulanmış yorum mu?
        /// true ise gerçekten rezervasyon yapıp tamamlamış
        /// false ise sadece kayıtlı kullanıcı (rezervasyon yapmamış)
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Admin onayı
        /// Tüm yorumlar moderasyon sürecinden geçebilir
        /// false ise frontend'de gösterilmez
        /// Spam/hakaret kontrolü için
        /// </summary>
        public bool IsApproved { get; set; } = false;

        // ============================================
        // FOREIGN KEYS
        // ============================================

        /// <summary>
        /// Yorumu yazan kullanıcı
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Yorumlanan araba
        /// </summary>
        public Guid CarId { get; set; }

        /// <summary>
        /// İlgili rezervasyon
        /// Bir rezervasyon için sadece 1 yorum bırakılabilir
        /// </summary>
        public Guid BookingId { get; set; }

        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================

        public User User { get; set; } = null!;
        public Car Car { get; set; } = null!;
        public Booking Booking { get; set; } = null!;
    }
}
