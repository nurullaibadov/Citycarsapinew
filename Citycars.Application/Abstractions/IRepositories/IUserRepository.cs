using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Email'e göre kullanıcı bul
        /// </summary>
        Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Email var mı kontrol et
        /// </summary>
        Task<bool> IsEmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Telefona göre kullanıcı bul
        /// </summary>
        Task<User?> GetByPhoneNumberAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default);
    }
}
