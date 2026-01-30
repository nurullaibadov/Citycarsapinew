using AutoMapper;
using Citycars.Application.Abstractions.IServices;
using Citycars.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Services (sonra ekleyeceğiz)
            // services.AddScoped<IAuthService, AuthService>();


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IBookingService, BookingService>();
            return services;
        }
    }
}
