using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class UpdateDoctorCertificateValidator : AbstractValidator<UpdateDoctorCertificateDto>
    {
        public UpdateDoctorCertificateValidator()
        {
            RuleFor(c => c.CertificateName)
                                .MinimumLength(3).WithMessage("Certificate name must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Certificate name cannot exceed 100 characters.").When(a => a.CertificateName != null);

            RuleFor(c => c.IssuingOrganization)
                .MinimumLength(3).WithMessage("Issuing organization must be at least 3 characters")
                .MaximumLength(150).WithMessage("Issuing organization cannot exceed 150 characters.")
                .When(a => a.IssuingOrganization != null);

            RuleFor(c => c.GraduationDate)
                .LessThan(DateTime.Now).WithMessage("Graduation date must be in the past")
                .When(c => c.GraduationDate.HasValue);
        }
    }
}
