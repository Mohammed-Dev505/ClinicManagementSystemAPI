using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorDto>
    {
        public UpdateDoctorValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("Doctor name is required").NotNull()
                                .MinimumLength(3).WithMessage("Doctor name must be at least 3 characters")
                                .MaximumLength(50).WithMessage("Doctor name cannot exceed 50 characters");

            RuleFor(a => a.PhoneNumber).NotEmpty().WithMessage("Phone number is required").NotNull()
                                       .MinimumLength(10).WithMessage("Phone number must be at least 10 digits")
                                       .MaximumLength(15).WithMessage("Phone number cannot exceed 15 digits");
        }
    }
}
