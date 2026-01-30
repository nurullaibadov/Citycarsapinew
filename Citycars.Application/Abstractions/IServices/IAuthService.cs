using Citycars.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        Task<TokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kullanıcı kaydı
        /// </summary>
        Task<TokenResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Refresh token ile yeni access token al
        /// </summary>
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Şifre değiştir
        /// </summary>
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

        /// <summary>
        /// Şifre sıfırlama linki gönder
        /// </summary>
        Task<bool> SendPasswordResetLinkAsync(string email, CancellationToken cancellationToken = default);
    }
}
