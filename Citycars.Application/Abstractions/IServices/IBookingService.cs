using Citycars.Application.DTOs.Booking;
using Citycars.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IServices
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(Guid userId, CreateBookingDto dto, CancellationToken cancellationToken = default);
        Task<BookingDto> GetBookingByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BookingDto> GetBookingByNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
        Task<List<BookingListDto>> GetUserBookingsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<PaginatedResult<BookingListDto>> GetAllBookingsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<bool> CancelBookingAsync(Guid bookingId, Guid userId, string reason, CancellationToken cancellationToken = default);
        Task<bool> ConfirmBookingAsync(Guid bookingId, CancellationToken cancellationToken = default);
        Task<bool> CompleteBookingAsync(Guid bookingId, CancellationToken cancellationToken = default);
    }
}
