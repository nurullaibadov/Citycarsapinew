using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Brand : BaseEntity
    {
        /// <summary>
        /// Marka adı
        /// Örnek: "Mercedes-Benz", "BMW", "Tesla"
        /// Unique olmalı
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Marka logosu
        /// Örnek: "https://cdn.citycars.az/brands/mercedes.svg"
        /// SVG formatı tercih edilir (scalable)
        /// Transparent background
        /// </summary>
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Marka hakkında bilgi
        /// Örnek: "Alman lüks otomobil üreticisi"
        /// SEO için
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Marka aktif mi?
        /// Pasif markalar frontend'de gösterilmez
        /// Örnek: Artık çalışmayan marka
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================

        /// <summary>
        /// Bu markanın arabaları
        /// One-to-Many: 1 Brand → Birden fazla Car
        /// </summary>
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
