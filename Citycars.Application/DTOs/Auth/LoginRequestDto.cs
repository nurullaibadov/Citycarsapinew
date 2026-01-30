using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        /// <summary>
        /// Email adresi
        /// Örnek: "user@example.com"
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre (plain text)
        /// Hash'lenecek ve database'deki ile karşılaştırılacak
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
