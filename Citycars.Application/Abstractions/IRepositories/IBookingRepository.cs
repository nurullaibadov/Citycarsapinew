using Citycars.Domain.Entities;
using Citycars.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IRepositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        /// <summary>
        /// Kullanıcının rezervasyonları
        /// </summary>
        Task<List<Booking>> GetUserBookingsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Arabanın rezervasyonları
        /// </summary>
        Task<List<Booking>> GetCarBookingsAsync(
            Guid carId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Tarih çakışması kontrolü
        /// </summary>
        Task<bool> HasDateConflictAsync(
            Guid carId,
            DateTime pickupDate,
            DateTime returnDate,
            Guid? excludeBookingId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Rezervasyon numarasına göre
        /// </summary>
        Task<Booking?> GetByBookingNumberAsync(
            string bookingNumber,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Detaylı rezervasyon bilgisi
        /// </summary>
        Task<Booking?> GetBookingWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Duruma göre rezervasyonlar
        /// </summary>
        Task<List<Booking>> GetBookingsByStatusAsync(
            BookingStatus status,
            CancellationToken cancellationToken = default);
    }
}
