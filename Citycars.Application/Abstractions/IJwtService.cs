using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions
{
    public interface IJwtService
    {
        /// <summary>
        /// Access token üret
        /// </summary>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Refresh token üret
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Token'dan kullanıcı ID'si al
        /// </summary>
        Guid? GetUserIdFromToken(string token);

        /// <summary>
        /// Token geçerli mi?
        /// </summary>
        bool ValidateToken(string token);
    }
}
