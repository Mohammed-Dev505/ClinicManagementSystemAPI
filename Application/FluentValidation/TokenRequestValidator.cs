using ClinicManagement.API.Application.Models;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class TokenRequestValidator : AbstractValidator<TokenRequestModel>
    {
        public TokenRequestValidator()
        {
            RuleFor(e => e.Email).NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .Must(email => email == email?.Trim())
                .WithMessage("Email must not contain leading or trailing space.");

            RuleFor(p => p.Password).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[\\d]").WithMessage("Password must contain at least ont digit.")
                .Matches("[\\W_]").WithMessage("Password must contain at least one special characters.");
        }
    }
}
