using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Enums
{
    public enum CarStatus
    {
        /// <summary>
        /// Müsait
        /// Kiralanabilir durumda
        /// Frontend'de gösterilir
        /// </summary>
        Available = 1,

        /// <summary>
        /// Rezerve edilmiş
        /// Aktif rezervasyonu var
        /// Frontend'de "Müsait değil" olarak gösterilir
        /// </summary>
        Booked = 2,

        /// <summary>
        /// Bakımda
        /// Servis, tamir, vs.
        /// Geçici olarak kiralanmaz
        /// </summary>
        InMaintenance = 3,

        /// <summary>
        /// Hizmet dışı
        /// Kalıcı olarak devre dışı
        /// Örnek: Hasar gördü, satıldı
        /// Frontend'de gösterilmez
        /// </summary>
        OutOfService = 4
    }
}
