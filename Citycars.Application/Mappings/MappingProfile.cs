using AutoMapper;
using Citycars.Application.DTOs.Auth;
using Citycars.Application.DTOs.Booking;
using Citycars.Application.DTOs.Car;
using Citycars.Application.DTOs.Category;
using Citycars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Citycars.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================
            // USER MAPPINGS
            // ============================================

            CreateMap<User, UserDto>();
            CreateMap<User, UserBookingDto>();
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password hash ayrı yapılacak

            // ============================================
            // CAR MAPPINGS
            // ============================================
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Features, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrls, opt => opt.Ignore());

            CreateMap<Car, CarListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));

            CreateMap<Car, CarBookingDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));

            CreateMap<Car, CarDto>()
      .ForMember(dest => dest.Features, opt => opt.Ignore())
      .ForMember(dest => dest.ImageUrls, opt => opt.Ignore());
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Features, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrls, opt => opt.Ignore());

            // ============================================
            // CATEGORY MAPPINGS
            // ============================================

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryDetailDto>()
                .ForMember(dest => dest.TotalCars, opt => opt.MapFrom(src => src.Cars.Count));
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // ============================================
            // BRAND MAPPINGS
            // ============================================

            CreateMap<Brand, BrandDto>();

            // ============================================
            // LOCATION MAPPINGS
            // ============================================

            CreateMap<Location, LocationDto>();

            // ============================================
            // BOOKING MAPPINGS
            // ============================================

            CreateMap<Booking, BookingDto>();
            CreateMap<Booking, BookingListDto>()
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Car.Model))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Car.Brand.Name))
                .ForMember(dest => dest.CarImageUrl, opt => opt.MapFrom(src => src.Car.MainImageUrl));
            CreateMap<CreateBookingDto, Booking>();

            // ============================================
            // PAYMENT MAPPINGS
            // ============================================

            CreateMap<Payment, PaymentDto>();
        }
    }
}


