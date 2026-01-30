using Citycars.Application.Abstractions.IRepositories;
using Citycars.Domain.Entities;
using Citycars.Domain.Enums;
using Citycars.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Booking>> GetUserBookingsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(b => b.UserId == userId)
                .Include(b => b.Car)
                    .ThenInclude(c => c.Brand)
                .Include(b => b.PickupLocation)
                .Include(b => b.ReturnLocation)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Booking>> GetCarBookingsAsync(
            Guid carId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(b => b.CarId == carId)
                .Include(b => b.User)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.PickupDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasDateConflictAsync(
            Guid carId,
            DateTime pickupDate,
            DateTime returnDate,
            Guid? excludeBookingId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(b =>
                b.CarId == carId &&
                b.Status != BookingStatus.Cancelled &&
                (
                    (b.PickupDate >= pickupDate && b.PickupDate < returnDate) ||
                    (b.ReturnDate > pickupDate && b.ReturnDate <= returnDate) ||
                    (b.PickupDate <= pickupDate && b.ReturnDate >= returnDate)
                )
            );

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<Booking?> GetByBookingNumberAsync(
            string bookingNumber,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(b => b.Car)
                .Include(b => b.User)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber, cancellationToken);
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(b => b.Car)
                    .ThenInclude(c => c.Brand)
                .Include(b => b.Car)
                    .ThenInclude(c => c.Category)
                .Include(b => b.User)
                .Include(b => b.PickupLocation)
                .Include(b => b.ReturnLocation)
                .Include(b => b.Payment)
                .Include(b => b.Review)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<Booking>> GetBookingsByStatusAsync(
            BookingStatus status,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(b => b.Status == status)
                .Include(b => b.Car)
                .Include(b => b.User)
                .ToListAsync(cancellationToken);
        }


        public async Task<int> CountByPrefixAsync(
    string prefix,
    CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(b => b.BookingNumber.StartsWith(prefix))
                .CountAsync(cancellationToken);
        }


        public async Task<List<Booking>> GetBookingsWithCarAndUserAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(b => b.Car)
                    .ThenInclude(c => c.Brand)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync(cancellationToken);
        }

    }
}
