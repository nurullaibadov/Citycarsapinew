using Citycars.Application.Abstractions.IRepositories;
using Citycars.Domain.Entities;
using Citycars.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        // Lazy initialization için
        private IUserRepository? _userRepository;
        private ICarRepository? _carRepository;
        private IBookingRepository? _bookingRepository;
        private IGenericRepository<Category>? _categoryRepository;
        private IGenericRepository<Brand>? _brandRepository;
        private IGenericRepository<Location>? _locationRepository;
        private IGenericRepository<Payment>? _paymentRepository;
        private IGenericRepository<Review>? _reviewRepository;
        private IGenericRepository<Setting>? _settingRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================
        // REPOSITORIES (Lazy Loading)
        // ============================================

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public ICarRepository Cars =>
            _carRepository ??= new CarRepository(_context);

        public IBookingRepository Bookings =>
            _bookingRepository ??= new BookingRepository(_context);

        public IGenericRepository<Category> Categories =>
            _categoryRepository ??= new GenericRepository<Category>(_context);

        public IGenericRepository<Brand> Brands =>
            _brandRepository ??= new GenericRepository<Brand>(_context);

        public IGenericRepository<Location> Locations =>
            _locationRepository ??= new GenericRepository<Location>(_context);

        public IGenericRepository<Payment> Payments =>
            _paymentRepository ??= new GenericRepository<Payment>(_context);

        public IGenericRepository<Review> Reviews =>
            _reviewRepository ??= new GenericRepository<Review>(_context);

        public IGenericRepository<Setting> Settings =>
            _settingRepository ??= new GenericRepository<Setting>(_context);

        // ============================================
        // METHODS
        // ============================================

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
