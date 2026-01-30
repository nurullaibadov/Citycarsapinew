using Citycars.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Abstractions.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // ============================================
        // QUERY METHODS (READ)
        // ============================================

        /// <summary>
        /// ID'ye göre getir
        /// </summary>
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tümünü getir
        /// </summary>
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Filtreye göre getir
        /// Örnek: x => x.IsActive
        /// </summary>
        Task<List<T>> GetWhereAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// İlk kaydı getir (filter ile)
        /// </summary>
        Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Kayıt var mı?
        /// </summary>
        Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sayaç
        /// </summary>
        Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// IQueryable döndür (kompleks sorgular için)
        /// </summary>
        IQueryable<T> GetQueryable();

        // ============================================
        // COMMAND METHODS (WRITE)
        // ============================================

        /// <summary>
        /// Yeni kayıt ekle
        /// </summary>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Çoklu kayıt ekle
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Güncelle
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Çoklu güncelle
        /// </summary>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Sil (Soft Delete)
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Çoklu sil
        /// </summary>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Kalıcı sil (Hard Delete - dikkatli kullan!)
        /// </summary>
        void HardDelete(T entity);
    }
}
