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
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Car>> GetAvailableCarsAsync(
            DateTime pickupDate,
            DateTime returnDate,
            CancellationToken cancellationToken = default)
        {
            // Belirtilen tarih aralığında rezervasyonu olmayan araçlar
            return await _dbSet
                .Where(c => c.Status == CarStatus.Available)
                .Where(c => !c.Bookings.Any(b =>
                    b.Status != BookingStatus.Cancelled &&
                    (
                        (b.PickupDate >= pickupDate && b.PickupDate < returnDate) ||
                        (b.ReturnDate > pickupDate && b.ReturnDate <= returnDate) ||
                        (b.PickupDate <= pickupDate && b.ReturnDate >= returnDate)
                    )
                ))
                .Include(c => c.Category)
                .Include(c => c.Brand)
                .Include(c => c.Location)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Car>> GetCarsByCategoryAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.CategoryId == categoryId && c.Status == CarStatus.Available)
                .Include(c => c.Brand)
                .Include(c => c.Location)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Car>> GetCarsByBrandAsync(
            Guid brandId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.BrandId == brandId && c.Status == CarStatus.Available)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Car>> GetFeaturedCarsAsync(
            int count = 10,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.IsFeatured && c.Status == CarStatus.Available)
                .Include(c => c.Category)
                .Include(c => c.Brand)
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Car>> GetCarsByPriceRangeAsync(
            decimal minPrice,
            decimal maxPrice,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.PricePerDay >= minPrice && c.PricePerDay <= maxPrice)
                .Where(c => c.Status == CarStatus.Available)
                .Include(c => c.Category)
                .Include(c => c.Brand)
                .ToListAsync(cancellationToken);
        }

        public async Task<Car?> GetCarWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.Brand)
                .Include(c => c.Location)
                .Include(c => c.Reviews.Where(r => r.IsApproved))
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
