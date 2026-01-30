using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IRepositories
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        /// <summary>
        /// Müsait araçları getir (tarih aralığında rezervasyonu olmayan)
        /// </summary>
        Task<List<Car>> GetAvailableCarsAsync(
            DateTime pickupDate,
            DateTime returnDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Kategoriye göre araçlar
        /// </summary>
        Task<List<Car>> GetCarsByCategoryAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Markaya göre araçlar
        /// </summary>
        Task<List<Car>> GetCarsByBrandAsync(
            Guid brandId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Öne çıkan araçlar
        /// </summary>
        Task<List<Car>> GetFeaturedCarsAsync(
            int count = 10,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Fiyat aralığına göre
        /// </summary>
        Task<List<Car>> GetCarsByPriceRangeAsync(
            decimal minPrice,
            decimal maxPrice,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Araba detayı (ilişkilerle birlikte)
        /// </summary>
        Task<Car?> GetCarWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
