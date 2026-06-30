using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class UpdateSpecialtyValidator : AbstractValidator<UpdateSpecialtyDto>
    {
        public UpdateSpecialtyValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("Specialty name is required").NotNull()
                                .MinimumLength(5).WithMessage("Specialty name must be at least 5 characters")
                                .MaximumLength(100).WithMessage("Specialty name cannot exceed 100 characters");
        }
    }
}
