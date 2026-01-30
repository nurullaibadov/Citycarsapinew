using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Domain.Entities
{
    public class Category : BaseEntity
    {
        /// <summary>
        /// Kategori adı
        /// Örnek: "Luxury Cars", "SUV", "Electric Vehicles"
        /// Unique olmalı
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kategori açıklaması
        /// Örnek: "Premium lüks araçlar, konforlu yolculuklar için"
        /// Nullable - opsiyonel
        /// SEO için önemli
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Kategori görseli
        /// Örnek: "https://cdn.citycars.az/categories/luxury.jpg"
        /// Frontend'de kartlarda gösterilecek
        /// Nullable - opsiyonel
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Sıralama numarası
        /// Frontend'de kategorileri sıralamak için
        /// Örnek: Luxury = 1, SUV = 2, Economy = 3
        /// Admin panelde drag-drop ile değiştirilebilir
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Kategori aktif mi?
        /// Pasif kategoriler frontend'de gösterilmez
        /// Ama database'de kalır (soft delete değil)
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================

        /// <summary>
        /// Bu kategorideki arabalar
        /// One-to-Many: 1 Category → Birden fazla Car
        /// </summary>
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
