using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Enums
{
    public enum PaymentStatus
    {
        /// <summary>
        /// Beklemede
        /// Ödeme henüz başlatılmadı
        /// </summary>
        Pending = 1,

        /// <summary>
        /// İşleniyor
        /// Payment gateway'e istek gönderildi
        /// Sonuç bekleniyor
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Tamamlandı
        /// Ödeme başarılı
        /// Para hesaba geçti
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Başarısız
        /// Ödeme reddedildi
        /// Örnek: Yetersiz bakiye, kart geçersiz
        /// </summary>
        Failed = 4,

        /// <summary>
        /// İade edildi
        /// Para kullanıcıya iade edildi
        /// Rezervasyon iptali durumunda
        /// </summary>
        Refunded = 5
    }
}
