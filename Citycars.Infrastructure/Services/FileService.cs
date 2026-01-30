using Citycars.Application.Abstractions.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _uploadPath;
        private readonly long _maxFileSize; // bytes
        private readonly string[] _allowedExtensions;

        public FileService(IConfiguration configuration)
        {
            var fileSettings = configuration.GetSection("FileSettings");
            _uploadPath = fileSettings["UploadPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            _maxFileSize = long.Parse(fileSettings["MaxFileSizeMB"] ?? "10") * 1024 * 1024; // MB to bytes
            _allowedExtensions = fileSettings.GetSection("AllowedExtensions").Get<string[]>()
                ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };

            // Upload klasörü yoksa oluştur
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        /// <summary>
        /// Dosya yükle
        /// </summary>
        public async Task<string> UploadFileAsync(IFormFile file, string folder = "uploads")
        {
            // ============================================
            // VALİDASYON
            // ============================================

            if (file == null || file.Length == 0)
                throw new ArgumentException("Dosya boş olamaz");

            // Dosya boyutu kontrolü
            if (file.Length > _maxFileSize)
                throw new ArgumentException($"Dosya boyutu {_maxFileSize / 1024 / 1024}MB'den büyük olamaz");

            // Extension kontrolü
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException($"Geçersiz dosya formatı. İzin verilen: {string.Join(", ", _allowedExtensions)}");

            // ============================================
            // DOSYA KAYDET
            // ============================================

            // Unique dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}{extension}";

            // Alt klasör oluştur (örnek: uploads/cars/)
            var folderPath = Path.Combine(_uploadPath, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Tam dosya yolu
            var filePath = Path.Combine(folderPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Relative path döndür (database'e kaydedilecek)
            return $"/{folder}/{fileName}";
        }

        /// <summary>
        /// Çoklu dosya yükle
        /// </summary>
        public async Task<List<string>> UploadFilesAsync(IEnumerable<IFormFile> files, string folder = "uploads")
        {
            var filePaths = new List<string>();

            foreach (var file in files)
            {
                var filePath = await UploadFileAsync(file, folder);
                filePaths.Add(filePath);
            }

            return filePaths;
        }

        /// <summary>
        /// Dosya sil
        /// </summary>
        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                // Relative path'i full path'e çevir
                var fullPath = Path.Combine(_uploadPath, filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Dosya var mı?
        /// </summary>
        public bool FileExists(string filePath)
        {
            var fullPath = Path.Combine(_uploadPath, filePath.TrimStart('/'));
            return File.Exists(fullPath);
        }

        /// <summary>
        /// Dosya boyutu
        /// </summary>
        public long GetFileSize(string filePath)
        {
            var fullPath = Path.Combine(_uploadPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                return new FileInfo(fullPath).Length;
            }
            return 0;
        }
    }
}
