using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Setting : BaseEntity
    {
        /// <summary>
        /// Ayar anahtarı
        /// Örnek: "SiteName", "ContactEmail", "MaxBookingDays"
        /// Unique olmalı
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Ayar değeri
        /// Örnek: "City Cars AZ", "info@citycars.az", "30"
        /// String olarak saklanır, gerekirse parse edilir
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Açıklama
        /// Admin panelde ne işe yaradığını gösterir
        /// Örnek: "Maksimum kaç gün önceden rezervasyon yapılabilir"
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Veri tipi
        /// Örnek: "String", "Number", "Boolean", "JSON"
        /// Frontend'de doğru input tipini göstermek için
        /// </summary>
        public string Type { get; set; } = "String";
    }
}
