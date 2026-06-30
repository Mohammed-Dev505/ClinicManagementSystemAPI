using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class CreateSpecialtyValidator : AbstractValidator<CreateSpecialtyDto>
    {
        public CreateSpecialtyValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("Specialty name is required").MinimumLength(5).WithMessage("Specialty name must be at least 5 characters.")
                                .MaximumLength(100).WithMessage("Specialty name cannot exceed 100 characters.").NotNull();
        }
    }
}
