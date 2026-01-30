using Citycars.Application.DTOs.Car;
using Citycars.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IServices
{
    public interface ICarService
    {
        Task<PaginatedResult<CarListDto>> GetAllCarsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<CarDto> GetCarByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<CarListDto>> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate, CancellationToken cancellationToken = default);
        Task<List<CarListDto>> GetCarsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task<List<CarListDto>> GetCarsByBrandAsync(Guid brandId, CancellationToken cancellationToken = default);
        Task<List<CarListDto>> GetFeaturedCarsAsync(int count = 10, CancellationToken cancellationToken = default);
        Task<CarDto> CreateCarAsync(CreateCarDto dto, CancellationToken cancellationToken = default);
        Task<CarDto> UpdateCarAsync(UpdateCarDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteCarAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<CarListDto>> SearchCarsAsync(string? searchTerm, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
