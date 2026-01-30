using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IServices
{
    public interface IFileService
    {
        /// <summary>
        /// Dosya yükle
        /// </summary>
        Task<string> UploadFileAsync(IFormFile file, string folder = "uploads");

        /// <summary>
        /// Çoklu dosya yükle
        /// </summary>
        Task<List<string>> UploadFilesAsync(IEnumerable<IFormFile> files, string folder = "uploads");

        /// <summary>
        /// Dosya sil
        /// </summary>
        Task<bool> DeleteFileAsync(string filePath);

        /// <summary>
        /// Dosya var mı kontrol et
        /// </summary>
        bool FileExists(string filePath);

        /// <summary>
        /// Dosya boyutu al (bytes)
        /// </summary>
        long GetFileSize(string filePath);
    }
}
