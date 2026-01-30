using AutoMapper;
using Citycars.Application.Abstractions.IRepositories;
using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Car;
using Citycars.Application.DTOs.Common;
using Citycars.Application.Exceptions;
using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CarListDto>> GetAllCarsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Cars.GetQueryable()
                .Include(c => c.Category)
                .Include(c => c.Brand)
                .OrderByDescending(c => c.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var cars = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var carDtos = _mapper.Map<List<CarListDto>>(cars);

            return new PaginatedResult<CarListDto>
            {
                Items = carDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<CarDto> GetCarByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var car = await _unitOfWork.Cars.GetCarWithDetailsAsync(id, cancellationToken);

            if (car == null)
                throw new NotFoundException("Car", id);

            return _mapper.Map<CarDto>(car);
        }

        public async Task<List<CarListDto>> GetAvailableCarsAsync(
            DateTime pickupDate,
            DateTime returnDate,
            CancellationToken cancellationToken = default)
        {
            var cars = await _unitOfWork.Cars.GetAvailableCarsAsync(pickupDate, returnDate, cancellationToken);
            return _mapper.Map<List<CarListDto>>(cars);
        }

        public async Task<List<CarListDto>> GetCarsByCategoryAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            var cars = await _unitOfWork.Cars.GetCarsByCategoryAsync(categoryId, cancellationToken);
            return _mapper.Map<List<CarListDto>>(cars);
        }

        public async Task<List<CarListDto>> GetCarsByBrandAsync(
            Guid brandId,
            CancellationToken cancellationToken = default)
        {
            var cars = await _unitOfWork.Cars.GetCarsByBrandAsync(brandId, cancellationToken);
            return _mapper.Map<List<CarListDto>>(cars);
        }

        public async Task<List<CarListDto>> GetFeaturedCarsAsync(
            int count = 10,
            CancellationToken cancellationToken = default)
        {
            var cars = await _unitOfWork.Cars.GetFeaturedCarsAsync(count, cancellationToken);
            return _mapper.Map<List<CarListDto>>(cars);
        }

        public async Task<CarDto> CreateCarAsync(CreateCarDto dto, CancellationToken cancellationToken = default)
        {
            // Kategori ve marka var mı kontrol et
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId, cancellationToken);
            if (category == null)
                throw new NotFoundException("Category", dto.CategoryId);

            var brand = await _unitOfWork.Brands.GetByIdAsync(dto.BrandId, cancellationToken);
            if (brand == null)
                throw new NotFoundException("Brand", dto.BrandId);

            // Plaka unique mi?
            var existingCar = await _unitOfWork.Cars.GetQueryable()
                .FirstOrDefaultAsync(c => c.LicensePlate == dto.LicensePlate, cancellationToken);

            if (existingCar != null)
                throw new BadRequestException("License plate already exists");

            // Car entity oluştur
            var car = _mapper.Map<Car>(dto);

            await _unitOfWork.Cars.AddAsync(car, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Detaylarıyla birlikte getir ve döndür
            var createdCar = await _unitOfWork.Cars.GetCarWithDetailsAsync(car.Id, cancellationToken);
            return _mapper.Map<CarDto>(createdCar!);
        }

        public async Task<CarDto> UpdateCarAsync(UpdateCarDto dto, CancellationToken cancellationToken = default)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(dto.Id, cancellationToken);
            if (car == null)
                throw new NotFoundException("Car", dto.Id);

            // Plaka değiştiyse ve başka arabada kullanılıyorsa hata ver
            if (car.LicensePlate != dto.LicensePlate)
            {
                var existingCar = await _unitOfWork.Cars.GetQueryable()
                    .FirstOrDefaultAsync(c => c.LicensePlate == dto.LicensePlate && c.Id != dto.Id, cancellationToken);

                if (existingCar != null)
                    throw new BadRequestException("License plate already exists");
            }

            _mapper.Map(dto, car);
            _unitOfWork.Cars.Update(car);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedCar = await _unitOfWork.Cars.GetCarWithDetailsAsync(car.Id, cancellationToken);
            return _mapper.Map<CarDto>(updatedCar!);
        }

        public async Task<bool> DeleteCarAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
            if (car == null)
                throw new NotFoundException("Car", id);

            // Aktif rezervasyonu var mı kontrol et
            var hasActiveBookings = await _unitOfWork.Bookings.GetQueryable()
                .AnyAsync(b => b.CarId == id &&
                    (b.Status == Domain.Enums.BookingStatus.Confirmed ||
                     b.Status == Domain.Enums.BookingStatus.InProgress),
                    cancellationToken);

            if (hasActiveBookings)
                throw new BadRequestException("Cannot delete car with active bookings");

            _unitOfWork.Cars.Delete(car); // Soft delete
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<PaginatedResult<CarListDto>> SearchCarsAsync(
            string? searchTerm,
            decimal? minPrice,
            decimal? maxPrice,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Cars.GetQueryable()
                .Include(c => c.Category)
                .Include(c => c.Brand)
                .AsQueryable();

            // Search term (model, brand name'de ara)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(c =>
                    c.Model.ToLower().Contains(lowerSearch) ||
                    c.Brand.Name.ToLower().Contains(lowerSearch));
            }

            // Fiyat filtreleri
            if (minPrice.HasValue)
                query = query.Where(c => c.PricePerDay >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var cars = await query
                .OrderByDescending(c => c.IsFeatured)
                .ThenByDescending(c => c.AverageRating)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var carDtos = _mapper.Map<List<CarListDto>>(cars);

            return new PaginatedResult<CarListDto>
            {
                Items = carDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
