using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IServices
{
    public interface IEmailService
    {
        /// <summary>
        /// Email gönder
        /// </summary>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);

        /// <summary>
        /// Rezervasyon onay maili
        /// </summary>
        Task SendBookingConfirmationEmailAsync(string to, string userName, string bookingNumber, DateTime pickupDate);

        /// <summary>
        /// Rezervasyon iptal maili
        /// </summary>
        Task SendBookingCancellationEmailAsync(string to, string userName, string bookingNumber);

        /// <summary>
        /// Şifre sıfırlama maili
        /// </summary>
        Task SendPasswordResetEmailAsync(string to, string userName, string resetToken);

        /// <summary>
        /// Email doğrulama maili
        /// </summary>
        Task SendEmailVerificationAsync(string to, string userName, string verificationToken);
    }
}
