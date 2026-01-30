using Citycars.Application.Abstractions.IServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var emailSettings = configuration.GetSection("EmailSettings");

            _smtpServer = emailSettings["SmtpServer"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            _smtpUsername = emailSettings["SmtpUsername"] ?? "";
            _smtpPassword = emailSettings["SmtpPassword"] ?? "";
            _fromEmail = emailSettings["FromEmail"] ?? "noreply@citycars.az";
            _fromName = emailSettings["FromName"] ?? "City Cars AZ";
        }

        /// <summary>
        /// Genel email gönderme metodu
        /// </summary>
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_fromName, _fromEmail));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                }
                else
                {
                    bodyBuilder.TextBody = body;
                }
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log hatayı (production'da logger kullan)
                Console.WriteLine($"Email gönderme hatası: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Rezervasyon onay maili
        /// </summary>
        public async Task SendBookingConfirmationEmailAsync(
            string to,
            string userName,
            string bookingNumber,
            DateTime pickupDate)
        {
            var subject = $"Rezervasyon Onayı - {bookingNumber}";
            var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2c3e50;'>Rezervasyonunuz Onaylandı!</h2>
                    <p>Sayın {userName},</p>
                    <p>Rezervasyonunuz başarıyla oluşturuldu.</p>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Rezervasyon Numarası:</strong> {bookingNumber}</p>
                        <p><strong>Teslim Alma Tarihi:</strong> {pickupDate:dd MMMM yyyy HH:mm}</p>
                    </div>
                    
                    <p>Araç teslim alırken lütfen kimlik belgenizi ve ehliyetinizi yanınızda bulundurun.</p>
                    
                    <p>İyi yolculuklar dileriz!</p>
                    
                    <hr style='margin-top: 30px; border: none; border-top: 1px solid #eee;'>
                    <p style='color: #7f8c8d; font-size: 12px;'>
                        City Cars AZ<br>
                        info@citycars.az<br>
                        +994 12 555 55 55
                    </p>
                </div>
            </body>
            </html>
        ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Rezervasyon iptal maili
        /// </summary>
        public async Task SendBookingCancellationEmailAsync(
            string to,
            string userName,
            string bookingNumber)
        {
            var subject = $"Rezervasyon İptali - {bookingNumber}";
            var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #e74c3c;'>Rezervasyonunuz İptal Edildi</h2>
                    <p>Sayın {userName},</p>
                    <p>Rezervasyon numarası <strong>{bookingNumber}</strong> olan rezervasyonunuz iptal edilmiştir.</p>
                    <p>Ödeme yaptıysanız, iade işlemi 3-5 iş günü içinde hesabınıza yapılacaktır.</p>
                    <p>Herhangi bir sorunuz için bizimle iletişime geçebilirsiniz.</p>
                    <hr style='margin-top: 30px;'>
                    <p style='color: #7f8c8d; font-size: 12px;'>City Cars AZ</p>
                </div>
            </body>
            </html>
        ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Şifre sıfırlama maili
        /// </summary>
        public async Task SendPasswordResetEmailAsync(
            string to,
            string userName,
            string resetToken)
        {
            var resetLink = $"https://citycars.az/reset-password?token={resetToken}";
            var subject = "Şifre Sıfırlama Talebi";
            var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2>Şifre Sıfırlama</h2>
                    <p>Sayın {userName},</p>
                    <p>Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:</p>
                    <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #3498db; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0;'>
                        Şifremi Sıfırla
                    </a>
                    <p>Bu link 1 saat geçerlidir.</p>
                    <p>Eğer bu talebi siz yapmadıysanız, bu maili görmezden gelebilirsiniz.</p>
                </div>
            </body>
            </html>
        ";

            await SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// Email doğrulama maili
        /// </summary>
        public async Task SendEmailVerificationAsync(
            string to,
            string userName,
            string verificationToken)
        {
            var verificationLink = $"https://citycars.az/verify-email?token={verificationToken}";
            var subject = "Email Adresinizi Doğrulayın";
            var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2>Hoş Geldiniz!</h2>
                    <p>Sayın {userName},</p>
                    <p>City Cars AZ'ye kaydolduğunuz için teşekkür ederiz.</p>
                    <p>Email adresinizi doğrulamak için aşağıdaki linke tıklayın:</p>
                    <a href='{verificationLink}' style='display: inline-block; padding: 10px 20px; background-color: #27ae60; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0;'>
                        Email Adresimi Doğrula
                    </a>
                </div>
            </body>
            </html>
        ";

            await SendEmailAsync(to, subject, body);
        }
    }
}
