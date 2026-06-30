using ClinicManagement.API.Application.Models;
using FluentValidation;
namespace ClinicManagement.API.Application.FluentValidation
{
    public class AddRoleValidator : AbstractValidator<AddRoleModel>
    {
        public AddRoleValidator()
        {
            RuleFor(i => i.UserId).NotEmpty().WithMessage("User ID is required");

            RuleFor(r => r.Role).NotEmpty().WithMessage("Role is required").NotNull();
        }
    }
}
