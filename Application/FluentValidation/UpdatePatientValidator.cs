using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class UpdatePatientValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("Patient name is required").NotNull()
                                .MinimumLength(3).WithMessage("Patient name must be at least 3 characters")
                                .MaximumLength(50).WithMessage("Patient name cannot exceed 50 characters");

            RuleFor(a => a.PhoneNumber).NotEmpty().WithMessage("Phone number is required").NotNull()
                                       .MinimumLength(10).WithMessage("Phone number must be at least 10 digits")
                                       .MaximumLength(15).WithMessage("Phone number cannot exceed 15 digit");
        }
    }
}
