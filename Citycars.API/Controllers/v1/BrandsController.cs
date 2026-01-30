using AutoMapper;
using Citycars.Application.Abstractions.IRepositories;
using Citycars.Application.DTOs.Car;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Citycars.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm markaları listele
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<BrandDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _unitOfWork.Brands.GetQueryable()
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();

            var brandDtos = _mapper.Map<List<BrandDto>>(brands);
            return Ok(ApiResponse<List<BrandDto>>.SuccessResponse(brandDtos));
        }
    }
}
