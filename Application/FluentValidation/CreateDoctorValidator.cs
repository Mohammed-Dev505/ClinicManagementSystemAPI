using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("Doctor name is required").MinimumLength(3).WithMessage("Doctor name must be at lease 3 characters.")
                                .MaximumLength(50).WithMessage("The name cannot exceed 50 characters").NotNull();

            RuleFor(ph => ph.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required").MinimumLength(10).WithMessage("PhoneNumber must be at lease 10 digits")
                                         .MaximumLength(15).WithMessage("PhoneNumber cannot exceed 15 digits.").NotNull();

            RuleFor(s => s.SpecialtyId).NotEmpty().WithMessage("The specialty id is required").NotNull();
        }
    }
}
