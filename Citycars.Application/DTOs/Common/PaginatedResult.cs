using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Common
{
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Mevcut sayfadaki öğeler
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Toplam kayıt sayısı
        /// Örnek: 156 araba
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Mevcut sayfa numarası
        /// 1'den başlar
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Sayfa başına kayıt sayısı
        /// Örnek: 12
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Toplam sayfa sayısı
        /// Hesaplama: Math.Ceiling(TotalCount / PageSize)
        /// Örnek: 156 / 12 = 13 sayfa
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Önceki sayfa var mı?
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Sonraki sayfa var mı?
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
