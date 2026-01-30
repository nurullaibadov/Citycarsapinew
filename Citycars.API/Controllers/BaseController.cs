using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Citycars.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Login olan kullanıcının ID'sini al
        /// JWT token'dan
        /// </summary>
        protected Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User ID not found in token");

            return userId;
        }

        /// <summary>
        /// Kullanıcı admin mi?
        /// </summary>
        protected bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        /// <summary>
        /// Kullanıcının email'ini al
        /// </summary>
        protected string GetUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }
}
