using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Auth;
using Citycars.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Citycars.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        /// <param name="dto">Email ve şifre</param>
        /// <returns>JWT token ve kullanıcı bilgileri</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(ApiResponse<TokenResponseDto>.SuccessResponse(result, "Login successful"));
        }

        /// <summary>
        /// Kullanıcı kaydı
        /// </summary>
        /// <param name="dto">Kayıt bilgileri</param>
        /// <returns>JWT token ve kullanıcı bilgileri</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(ApiResponse<TokenResponseDto>.SuccessResponse(result, "Registration successful"));
        }

        /// <summary>
        /// Şifre sıfırlama linki gönder
        /// </summary>
        /// <param name="email">Email adresi</param>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _authService.SendPasswordResetLinkAsync(email);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Password reset link sent to your email"));
        }
    }
}
