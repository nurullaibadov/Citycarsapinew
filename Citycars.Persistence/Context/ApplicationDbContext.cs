using Citycars.Domain.Entities;
using Citycars.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Categories tablosu
        /// Araç kategorileri (Luxury, SUV, Economy, Electric)
        /// </summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Brands tablosu
        /// Araç markaları (Mercedes, BMW, Tesla, vs.)
        /// </summary>
        public DbSet<Brand> Brands { get; set; } = null!;

        /// <summary>
        /// Locations tablosu
        /// Teslim alma/bırakma noktaları
        /// </summary>
        public DbSet<Location> Locations { get; set; } = null!;

        /// <summary>
        /// Cars tablosu
        /// Ana ürün tablosu
        /// </summary>
        public DbSet<Car> Cars { get; set; } = null!;

        /// <summary>
        /// Bookings tablosu
        /// Rezervasyonlar
        /// </summary>
        public DbSet<Booking> Bookings { get; set; } = null!;

        /// <summary>
        /// Payments tablosu
        /// Ödeme kayıtları
        /// </summary>
        public DbSet<Payment> Payments { get; set; } = null!;

        /// <summary>
        /// Reviews tablosu
        /// Kullanıcı yorumları
        /// </summary>
        public DbSet<Review> Reviews { get; set; } = null!;

        /// <summary>
        /// Settings tablosu
        /// Sistem ayarları (Key-Value)
        /// </summary>
        public DbSet<Setting> Settings { get; set; } = null!;

        // ============================================
        // MODEL CONFIGURATION
        // ============================================

        /// <summary>
        /// Model oluşturulurken çağrılır
        /// Entity Configurations burada uygulanır
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tüm IEntityTypeConfiguration'ları otomatik olarak uygula
            // Bu sayede her configuration'ı tek tek yazmamıza gerek yok
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Global Query Filters
            // Soft Delete için: IsDeleted=true olanları otomatik filtrele
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                        Expression.Constant(false)
                    );
                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        /// <summary>
        /// SaveChanges override
        /// Her kayıt/güncelleme öncesi otomatik işlemler
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Değişen entity'leri al
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Yeni kayıt ekleniyorsa
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.Id = Guid.NewGuid();
                        break;

                    case EntityState.Modified:
                        // Güncelleme yapılıyorsa
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        // Silme işlemi - Soft Delete yap
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
