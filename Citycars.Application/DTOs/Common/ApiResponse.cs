using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.DTOs.Common
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// İşlem başarılı mı?
        /// true = 200, false = 4xx/5xx
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Kullanıcıya gösterilecek mesaj
        /// Örnek: "Car created successfully", "Invalid email or password"
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Response data'sı
        /// Generic tip - herhangi bir DTO olabilir
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Hata mesajları (validasyon vs.)
        /// Örnek: ["Email is required", "Password must be at least 6 characters"]
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Başarılı response oluştur
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message ?? "Operation successful",
                Data = data
            };
        }

        /// <summary>
        /// Hata response oluştur
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
