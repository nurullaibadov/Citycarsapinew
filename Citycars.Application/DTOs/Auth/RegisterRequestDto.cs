using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        /// <summary>
        /// Ad
        /// Min: 2 karakter, Max: 50 karakter
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Soyad
        /// Min: 2 karakter, Max: 50 karakter
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// Unique olmalı
        /// Email formatında olmalı
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// Min: 6 karakter
        /// En az 1 büyük harf, 1 küçük harf, 1 rakam içermeli (validation)
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Şifre tekrarı
        /// Password ile aynı olmalı
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Telefon (opsiyonel)
        /// </summary>
        public string? PhoneNumber { get; set; }
    }
}
