using AutoMapper;
using Citycars.Application.Abstractions.IRepositories;
using Citycars.Application.DTOs.Auth;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Citycars.API.Controllers.v1.Admin
{
    [ApiController]
    [Route("api/v1/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminUsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm kullanıcıları listele
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(ApiResponse<List<UserDto>>.SuccessResponse(userDtos));
        }

        /// <summary>
        /// Kullanıcıyı aktif/pasif yap
        /// </summary>
        [HttpPatch("{id}/toggle-active")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ToggleActive(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<object>.ErrorResponse("User not found"));

            user.IsActive = !user.IsActive;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResponse(true, $"User {(user.IsActive ? "activated" : "deactivated")}"));
        }
    }
}
