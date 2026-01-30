using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Car;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Citycars.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Tüm araçları listele (paginated)
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası (default: 1)</param>
        /// <param name="pageSize">Sayfa boyutu (default: 12)</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<CarListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
        {
            var result = await _carService.GetAllCarsAsync(pageNumber, pageSize);
            return Ok(ApiResponse<PaginatedResult<CarListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Araç detayı
        /// </summary>
        /// <param name="id">Araç ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _carService.GetCarByIdAsync(id);
            return Ok(ApiResponse<CarDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Müsait araçlar (tarih aralığına göre)
        /// </summary>
        /// <param name="pickupDate">Teslim alma tarihi</param>
        /// <param name="returnDate">Teslim etme tarihi</param>
        [HttpGet("available")]
        [ProducesResponseType(typeof(ApiResponse<List<CarListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailable([FromQuery] DateTime pickupDate, [FromQuery] DateTime returnDate)
        {
            var result = await _carService.GetAvailableCarsAsync(pickupDate, returnDate);
            return Ok(ApiResponse<List<CarListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Öne çıkan araçlar
        /// </summary>
        /// <param name="count">Kaç adet (default: 10)</param>
        [HttpGet("featured")]
        [ProducesResponseType(typeof(ApiResponse<List<CarListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeatured([FromQuery] int count = 10)
        {
            var result = await _carService.GetFeaturedCarsAsync(count);
            return Ok(ApiResponse<List<CarListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Kategoriye göre araçlar
        /// </summary>
        /// <param name="categoryId">Kategori ID</param>
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(ApiResponse<List<CarListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var result = await _carService.GetCarsByCategoryAsync(categoryId);
            return Ok(ApiResponse<List<CarListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Markaya göre araçlar
        /// </summary>
        /// <param name="brandId">Marka ID</param>
        [HttpGet("brand/{brandId}")]
        [ProducesResponseType(typeof(ApiResponse<List<CarListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBrand(Guid brandId)
        {
            var result = await _carService.GetCarsByBrandAsync(brandId);
            return Ok(ApiResponse<List<CarListDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Araç arama (model, fiyat filtresi)
        /// </summary>
        /// <param name="searchTerm">Arama terimi (model, marka)</param>
        /// <param name="minPrice">Min fiyat</param>
        /// <param name="maxPrice">Max fiyat</param>
        /// <param name="pageNumber">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<CarListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 12)
        {
            var result = await _carService.SearchCarsAsync(searchTerm, minPrice, maxPrice, pageNumber, pageSize);
            return Ok(ApiResponse<PaginatedResult<CarListDto>>.SuccessResponse(result));
        }

    }
}