using Citycars.Application.DTOs.Booking;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citycars.Application.Validators.Booking
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.CarId)
                .NotEmpty().WithMessage("Car is required");

            RuleFor(x => x.PickupDate)
                .NotEmpty().WithMessage("Pickup date is required")
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(-1))
                .WithMessage("Pickup date cannot be in the past");

            RuleFor(x => x.ReturnDate)
                .NotEmpty().WithMessage("Return date is required")
                .GreaterThan(x => x.PickupDate)
                .WithMessage("Return date must be after pickup date");

            RuleFor(x => x)
                .Must(x => (x.ReturnDate - x.PickupDate).TotalHours >= 24)
                .WithMessage("Minimum rental period is 24 hours");

            RuleFor(x => x)
                .Must(x => (x.ReturnDate - x.PickupDate).TotalDays <= 30)
                .WithMessage("Maximum rental period is 30 days");

            RuleFor(x => x.PickupLocationId)
                .NotEmpty().WithMessage("Pickup location is required");

            RuleFor(x => x.ReturnLocationId)
                .NotEmpty().WithMessage("Return location is required");

            RuleFor(x => x.SpecialRequests)
                .MaximumLength(1000).WithMessage("Special requests cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.SpecialRequests));
        }
    }
}
