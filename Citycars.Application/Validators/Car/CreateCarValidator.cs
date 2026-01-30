using Citycars.Application.DTOs.Car;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Validators.Car
{
    public class CreateCarValidator : AbstractValidator<CreateCarDto>
    {
        public CreateCarValidator()
        {
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(100).WithMessage("Model cannot exceed 100 characters");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(2000).WithMessage("Year must be 2000 or later")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Year cannot be in the future");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Color is required")
                .MaximumLength(50);

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("License plate is required")
                .MaximumLength(20);

            RuleFor(x => x.Seats)
                .GreaterThan(0).WithMessage("Seats must be greater than 0")
                .LessThanOrEqualTo(50).WithMessage("Seats cannot exceed 50");

            RuleFor(x => x.FuelType)
                .NotEmpty().WithMessage("Fuel type is required")
                .Must(x => new[] { "Petrol", "Diesel", "Electric", "Hybrid" }.Contains(x))
                .WithMessage("Invalid fuel type");

            RuleFor(x => x.Transmission)
                .NotEmpty().WithMessage("Transmission is required")
                .Must(x => new[] { "Automatic", "Manual" }.Contains(x))
                .WithMessage("Invalid transmission type");

            RuleFor(x => x.PricePerDay)
                .GreaterThan(0).WithMessage("Price per day must be greater than 0")
                .LessThan(100000).WithMessage("Price per day seems unrealistic");

            RuleFor(x => x.PricePerHour)
                .GreaterThan(0).WithMessage("Price per hour must be greater than 0")
                .When(x => x.PricePerHour.HasValue);

            RuleFor(x => x.Mileage)
                .GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Brand is required");
        }
    }
}
