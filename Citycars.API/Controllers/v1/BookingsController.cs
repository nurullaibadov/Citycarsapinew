using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Booking;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Citycars.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // Tüm endpoint'ler login gerektirir
    public class BookingsController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Yeni rezervasyon oluştur
        /// </summary>
        /// <param name="dto">Rezervasyon bilgileri</param>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var userId = GetUserId(); // JWT'den user ID al
            var result = await _bookingService.CreateBookingAsync(userId, dto);
            return Ok(ApiResponse<BookingDto>.SuccessResponse(result, "Booking created successfully"));
        }

        /// <summary>
        /// Kullanıcının rezervasyonları
        /// </summary>
        [HttpGet("my-bookings")]
        [ProducesResponseType(typeof(ApiResponse<List<BookingListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = GetUserId();
            var result = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(ApiResponse<List<BookingListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Rezervasyon detayı
        /// </summary>
        /// <param name="id">Rezervasyon ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(ApiResponse<BookingDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Rezervasyon numarasına göre getir
        /// </summary>
        /// <param name="bookingNumber">Rezervasyon numarası (örn: BK-2024-00001)</param>
        [HttpGet("number/{bookingNumber}")]
        [ProducesResponseType(typeof(ApiResponse<BookingDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByNumber(string bookingNumber)
        {
            var result = await _bookingService.GetBookingByNumberAsync(bookingNumber);
            return Ok(ApiResponse<BookingDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Rezervasyonu iptal et
        /// </summary>
        /// <param name="id">Rezervasyon ID</param>
        /// <param name="reason">İptal nedeni</param>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason)
        {
            var userId = GetUserId();
            var result = await _bookingService.CancelBookingAsync(id, userId, reason);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Booking cancelled successfully"));
        }
    }
}
