using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Car;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Citycars.API.Controllers.v1.Admin
{
    [ApiController]
    [Route("api/v1/admin/[controller]")]
    [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
    public class AdminCarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public AdminCarsController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Yeni araç ekle
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateCarDto dto)
        {
            var result = await _carService.CreateCarAsync(dto);
            return Ok(ApiResponse<CarDto>.SuccessResponse(result, "Car created successfully"));
        }

        /// <summary>
        /// Araç güncelle
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCarDto dto)
        {
            dto.Id = id;
            var result = await _carService.UpdateCarAsync(dto);
            return Ok(ApiResponse<CarDto>.SuccessResponse(result, "Car updated successfully"));
        }

        /// <summary>
        /// Araç sil (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _carService.DeleteCarAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Car deleted successfully"));
        }
    }
}
