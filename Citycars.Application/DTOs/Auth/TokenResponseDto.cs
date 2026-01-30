using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Auth
{
    public class TokenResponseDto
    {
        /// <summary>
        /// JWT Access Token
        /// Her API isteğinde header'da gönderilecek
        /// Örnek: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh Token
        /// Access token expire olduğunda yeni token almak için
        /// Daha uzun ömürlü (örnek: 7 gün)
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Token ne zaman expire olacak?
        /// Frontend'de countdown göstermek için
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Token tipi
        /// Her zaman "Bearer"
        /// Authorization header: "Bearer {AccessToken}"
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Kullanıcı bilgileri
        /// Frontend'de kullanıcı adı göstermek için
        /// </summary>
        public UserDto User { get; set; } = null!;
    }
}
