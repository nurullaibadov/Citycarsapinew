using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        // ============================================
        // REPOSITORIES
        // ============================================

        IUserRepository Users { get; }
        ICarRepository Cars { get; }
        IBookingRepository Bookings { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Brand> Brands { get; }
        IGenericRepository<Location> Locations { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<Setting> Settings { get; }

        // ============================================
        // METHODS
        // ============================================

        /// <summary>
        /// Değişiklikleri kaydet (transaction commit)
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Transaction başlat
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Transaction commit
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Transaction rollback
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
