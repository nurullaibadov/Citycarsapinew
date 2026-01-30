using Citycars.Application.Abstractions.IRepositories;
using Citycars.Persistence.Context;
using Citycars.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ============================================
            // DATABASE CONNECTION
            // ============================================

            // Connection string'i appsettings.json'dan al
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // DbContext'i kaydet
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Migration assembly
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);

                    // Retry on failure (bağlantı koptuğunda tekrar dene)
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);

                    // Command timeout
                    sqlOptions.CommandTimeout(30);
                });

                // Development ortamında sensitive data logging
#if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });

            // ============================================
            // REPOSITORIES
            // ============================================

            // Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Specific Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            // ============================================
            // UNIT OF WORK
            // ============================================

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
