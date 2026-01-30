using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Enums
{
    public enum BookingStatus
    {
        /// <summary>
        /// Beklemede
        /// Rezervasyon oluşturuldu ama ödeme yapılmadı
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Onaylandı
        /// Ödeme yapıldı, rezervasyon kesinleşti
        /// Müşteriye onay maili gönderildi
        /// </summary>
        Confirmed = 2,

        /// <summary>
        /// Devam ediyor
        /// Araç teslim alındı, kullanımda
        /// </summary>
        InProgress = 3,

        /// <summary>
        /// Tamamlandı
        /// Araç teslim edildi, rezervasyon bitti
        /// Artık yorum bırakılabilir
        /// </summary>
        Completed = 4,

        /// <summary>
        /// İptal edildi
        /// Kullanıcı veya admin tarafından iptal edildi
        /// İade işlemi başlatılabilir
        /// </summary>
        Cancelled = 5
    }
}
