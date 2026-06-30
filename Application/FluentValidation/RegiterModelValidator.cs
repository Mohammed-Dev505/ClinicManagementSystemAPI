using ClinicManagement.API.Application.Models;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class RegiterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegiterModelValidator()
        {
            RuleFor(u => u.Username).NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(e => e.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .Must(email => email == email?.Trim())
                .WithMessage("Email must not contain leading or trailing space");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Password must be contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must be contain at least one lowercase letter")
                .Matches("[\\d]").WithMessage("Password must be contain at least one digit.")
                .Matches("[\\W_]").WithMessage("Password must be contain at least one special characters.");
        }
    }
}
