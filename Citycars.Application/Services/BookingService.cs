using AutoMapper;
using Citycars.Application.Abstractions.IRepositories;

using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Booking;
using Citycars.Application.DTOs.Common;
using Citycars.Application.Exceptions;
using Citycars.Domain.Entities;
using Citycars.Domain.Enums;

using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Services
{


    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public BookingService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// Yeni rezervasyon oluştur
        /// </summary>
        public async Task<BookingDto> CreateBookingAsync(
            Guid userId,
            CreateBookingDto dto,
            CancellationToken cancellationToken = default)
        {
            // ============================================
            // VALIDATION - Araba var mı?
            // ============================================
            var car = await _unitOfWork.Cars.GetByIdAsync(dto.CarId, cancellationToken);
            if (car == null)
                throw new NotFoundException("Car", dto.CarId);

            if (car.Status != CarStatus.Available)
                throw new BadRequestException("Car is not available");

            // ============================================
            // VALIDATION - Kullanıcı var mı?
            // ============================================
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException("User", userId);

            // ============================================
            // VALIDATION - Lokasyonlar var mı?
            // ============================================
            var pickupLocation = await _unitOfWork.Locations.GetByIdAsync(dto.PickupLocationId, cancellationToken);
            if (pickupLocation == null)
                throw new NotFoundException("Pickup Location", dto.PickupLocationId);

            var returnLocation = await _unitOfWork.Locations.GetByIdAsync(dto.ReturnLocationId, cancellationToken);
            if (returnLocation == null)
                throw new NotFoundException("Return Location", dto.ReturnLocationId);

            // ============================================
            // VALIDATION - Tarih çakışması var mı?
            // ============================================
            var hasConflict = await _unitOfWork.Bookings.HasDateConflictAsync(
                dto.CarId,
                dto.PickupDate,
                dto.ReturnDate,
                cancellationToken: cancellationToken);

            if (hasConflict)
                throw new BadRequestException("Car is already booked for selected dates");

            // ============================================
            // FİYAT HESAPLAMA
            // ============================================
            var totalDays = (int)(dto.ReturnDate - dto.PickupDate).TotalDays;
            if (totalDays < 1)
                totalDays = 1;

            var pricePerDay = car.PricePerDay;
            var totalPrice = pricePerDay * totalDays;
            var discountAmount = 0m;

            // İndirim hesapla (7 günden fazlaysa %10 indirim)
            if (totalDays >= 7)
            {
                discountAmount = totalPrice * 0.10m;
            }

            var finalPrice = totalPrice - discountAmount;

            // ============================================
            // BOOKING NUMBER OLUŞTUR
            // ============================================
            var bookingNumber = await GenerateBookingNumberAsync(cancellationToken);

            // ============================================
            // BOOKING ENTITY OLUŞTUR
            // ============================================
            var booking = new Booking
            {
                BookingNumber = bookingNumber,
                UserId = userId,
                CarId = dto.CarId,
                PickupLocationId = dto.PickupLocationId,
                ReturnLocationId = dto.ReturnLocationId,
                PickupDate = dto.PickupDate,
                ReturnDate = dto.ReturnDate,
                TotalDays = totalDays,
                PricePerDay = pricePerDay,
                TotalPrice = totalPrice,
                DiscountAmount = discountAmount > 0 ? discountAmount : null,
                FinalPrice = finalPrice,
                Status = BookingStatus.Pending,
                SpecialRequests = dto.SpecialRequests
            };

            await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);

            // ============================================
            // ARABA DURUMUNU GÜNCELLE
            // ============================================
            car.Status = CarStatus.Booked;
            _unitOfWork.Cars.Update(car);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // ============================================
            // EMAIL GÖNDER
            // ============================================
            try
            {
                await _emailService.SendBookingConfirmationEmailAsync(
                    user.Email,
                    user.FirstName,
                    bookingNumber,
                    dto.PickupDate
                );
            }
            catch (Exception ex)
            {
                // Email hatası booking'i etkilemesin, log'la
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }

            // ============================================
            // DETAYLIYLA BİRLİKTE GETİR VE DÖNDÜR
            // ============================================
            var createdBooking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(booking.Id, cancellationToken);
            return _mapper.Map<BookingDto>(createdBooking!);
        }

        /// <summary>
        /// Rezervasyonu ID'ye göre getir
        /// </summary>
        public async Task<BookingDto> GetBookingByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(id, cancellationToken);

            if (booking == null)
                throw new NotFoundException("Booking", id);

            return _mapper.Map<BookingDto>(booking);
        }

        /// <summary>
        /// Rezervasyon numarasına göre getir
        /// </summary>
        public async Task<BookingDto> GetBookingByNumberAsync(string bookingNumber, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetByBookingNumberAsync(bookingNumber, cancellationToken);

            if (booking == null)
                throw new NotFoundException($"Booking with number {bookingNumber} not found");

            return _mapper.Map<BookingDto>(booking);
        }

        /// <summary>
        /// Kullanıcının rezervasyonlarını getir
        /// </summary>
        public async Task<List<BookingListDto>> GetUserBookingsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(userId, cancellationToken);
            return _mapper.Map<List<BookingListDto>>(bookings);
        }

        /// <summary>
        /// Tüm rezervasyonları getir (Admin için)
        /// </summary>
        public async Task<PaginatedResult<BookingListDto>> GetAllBookingsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Bookings.GetQueryable()
                .Include(b => b.Car)
                    .ThenInclude(c => c.Brand)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var bookings = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var bookingDtos = _mapper.Map<List<BookingListDto>>(bookings);

            return new PaginatedResult<BookingListDto>
            {
                Items = bookingDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Rezervasyonu iptal et
        /// </summary>
        public async Task<bool> CancelBookingAsync(
            Guid bookingId,
            Guid userId,
            string reason,
            CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(bookingId, cancellationToken);

            if (booking == null)
                throw new NotFoundException("Booking", bookingId);

            // Sadece kendi rezervasyonunu iptal edebilir
            if (booking.UserId != userId)
                throw new UnauthorizedException("You can only cancel your own bookings");

            // Zaten iptal edilmiş veya tamamlanmışsa iptal edilemez
            if (booking.Status == BookingStatus.Cancelled)
                throw new BadRequestException("Booking is already cancelled");

            if (booking.Status == BookingStatus.Completed)
                throw new BadRequestException("Cannot cancel completed booking");

            // İptal et
            booking.Status = BookingStatus.Cancelled;
            booking.CancellationReason = reason;
            booking.CancellationDate = DateTime.UtcNow;

            _unitOfWork.Bookings.Update(booking);

            // Arabayı tekrar müsait yap
            var car = await _unitOfWork.Cars.GetByIdAsync(booking.CarId, cancellationToken);
            if (car != null)
            {
                car.Status = CarStatus.Available;
                _unitOfWork.Cars.Update(car);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // İptal maili gönder
            try
            {
                await _emailService.SendBookingCancellationEmailAsync(
                    booking.User.Email,
                    booking.User.FirstName,
                    booking.BookingNumber
                );
            }
            catch { }

            return true;
        }

        /// <summary>
        /// Rezervasyonu onayla (Admin)
        /// </summary>
        public async Task<bool> ConfirmBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId, cancellationToken);

            if (booking == null)
                throw new NotFoundException("Booking", bookingId);

            if (booking.Status != BookingStatus.Pending)
                throw new BadRequestException("Only pending bookings can be confirmed");

            booking.Status = BookingStatus.Confirmed;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Rezervasyonu tamamla (Admin)
        /// </summary>
        public async Task<bool> CompleteBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(bookingId, cancellationToken);

            if (booking == null)
                throw new NotFoundException("Booking", bookingId);

            if (booking.Status != BookingStatus.InProgress)
                throw new BadRequestException("Only in-progress bookings can be completed");

            booking.Status = BookingStatus.Completed;
            _unitOfWork.Bookings.Update(booking);

            // Arabayı tekrar müsait yap
            var car = await _unitOfWork.Cars.GetByIdAsync(booking.CarId, cancellationToken);
            if (car != null)
            {
                car.Status = CarStatus.Available;
                _unitOfWork.Cars.Update(car);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Unique booking number oluştur
        /// Format: BK-2024-00001
        /// </summary>
        private async Task<string> GenerateBookingNumberAsync(CancellationToken cancellationToken)
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"BK-{year}-";

            // Bu yıl kaç rezervasyon var?
            var count = await _unitOfWork.Bookings
      .CountByPrefixAsync(prefix, cancellationToken);

            var number = (count + 1).ToString("D5");
            return $"{prefix}{number}";


        }
    }
}