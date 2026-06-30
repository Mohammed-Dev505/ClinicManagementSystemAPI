using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class CreatePatientValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("Patient name is required").MinimumLength(3).WithMessage("Patient name must be at least 3 characters.")
                                 .MaximumLength(50).WithMessage("Patient name cannot exceed 50 characters.").NotNull();

            RuleFor(ph => ph.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required").MinimumLength(10).WithMessage("PhoneNumber must be at least 10 digits")
                                         .MaximumLength(15).WithMessage("PhoneNumber cannot exceed 15 digits.").NotNull();

            RuleFor(d => d.DateOfBirth).NotEmpty().WithMessage("DateOfBirth is required").LessThan(DateTime.Today).WithMessage("The date of birth cannot be in the future or today date")
                                        .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("The date of birth is illogical , Please confirm the year.");
        }
    }
}
