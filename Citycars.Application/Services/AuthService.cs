using AutoMapper;
using Citycars.Application.Abstractions;
using Citycars.Application.Abstractions.IRepositories;
using Citycars.Application.Abstractions.IServices;
using Citycars.Application.DTOs.Auth;
using Citycars.Application.Exceptions;
using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IMapper mapper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        public async Task<TokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            // Email'e göre kullanıcı bul
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email, cancellationToken);

            if (user == null)
                throw new UnauthorizedException("Invalid email or password");

            // Şifre kontrolü
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password");

            // Hesap aktif mi?
            if (!user.IsActive)
                throw new UnauthorizedException("Account is deactivated");

            // Token üret
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // UserDto oluştur
            var userDto = _mapper.Map<UserDto>(user);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = userDto
            };
        }

        /// <summary>
        /// Kullanıcı kaydı
        /// </summary>
        public async Task<TokenResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
        {
            // Email zaten kayıtlı mı?
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email, cancellationToken);
            if (existingUser != null)
                throw new BadRequestException("Email is already registered");

            // User entity oluştur
            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Role = "User";
            user.IsEmailVerified = false;
            user.IsActive = true;

            // Database'e ekle
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Email doğrulama maili gönder
            var verificationToken = Guid.NewGuid().ToString();
            await _emailService.SendEmailVerificationAsync(user.Email, user.FirstName, verificationToken);

            // Token üret ve döndür
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var userDto = _mapper.Map<UserDto>(user);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = userDto
            };
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            // TODO: Refresh token validation ve database'de saklanması
            // Şimdilik basit implementation
            throw new NotImplementedException("Refresh token feature will be implemented");
        }

        /// <summary>
        /// Şifre değiştir
        /// </summary>
        public async Task<bool> ChangePasswordAsync(
            Guid userId,
            string currentPassword,
            string newPassword,
            CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException("User", userId);

            // Mevcut şifre doğru mu?
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                throw new BadRequestException("Current password is incorrect");

            // Yeni şifreyi hash'le ve kaydet
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Şifre sıfırlama linki gönder
        /// </summary>
        public async Task<bool> SendPasswordResetLinkAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken);
            if (user == null)
                return true; // Güvenlik için: Email bulunamasa bile true döndür

            // Reset token üret
            var resetToken = Guid.NewGuid().ToString();

            // Email gönder
            await _emailService.SendPasswordResetEmailAsync(user.Email, user.FirstName, resetToken);

            return true;
        }
    }

}
