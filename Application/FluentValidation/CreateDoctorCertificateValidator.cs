using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class CreateDoctorCertificateValidator : AbstractValidator<CreateDoctorCertificateDto>
    {
        public CreateDoctorCertificateValidator()
        {
            RuleFor(c => c.CertificateName).NotEmpty().WithMessage("Certificate name is required.")
                .MinimumLength(3).WithMessage("Certificate name must be at least 3 characters.")
                .MaximumLength(100).WithMessage("Certificate name cannot exceed 100 characters.").NotNull();

            RuleFor(c => c.IssuingOrganization).NotEmpty().WithMessage("Issuing organization is required.")
                .MinimumLength(3).WithMessage("Issuing organization must be at least 3 characters.")
                .MaximumLength(150).WithMessage("Issuing organization cannot exceed 150 characters.").NotNull();

            RuleFor(c => c.GraduationDate).NotEmpty().WithMessage("Graduation date is required.")
                .LessThan(DateTime.Now).WithMessage("Graduation date must be in the past.").NotNull();

            RuleFor(c => c.DoctorId).NotEmpty().WithMessage("A valid Doctor ID must be assigned to this certificate.").NotNull();
        }
    }
}
