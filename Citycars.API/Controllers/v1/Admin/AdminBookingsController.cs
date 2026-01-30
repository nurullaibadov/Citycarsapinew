using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Booking;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Citycars.API.Controllers.v1.Admin
{
    [ApiController]
    [Route("api/v1/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminBookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public AdminBookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Tüm rezervasyonları listele
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<BookingListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _bookingService.GetAllBookingsAsync(pageNumber, pageSize);
            return Ok(ApiResponse<PaginatedResult<BookingListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Rezervasyonu onayla
        /// </summary>
        [HttpPost("{id}/confirm")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var result = await _bookingService.ConfirmBookingAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Booking confirmed"));
        }

        /// <summary>
        /// Rezervasyonu tamamla
        /// </summary>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Complete(Guid id)
        {
            var result = await _bookingService.CompleteBookingAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Booking completed"));
        }
    }
}
