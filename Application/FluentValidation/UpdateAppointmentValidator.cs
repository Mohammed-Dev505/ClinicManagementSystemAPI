using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(a => a.AppointmentDate).GreaterThanOrEqualTo(DateTime.Now).WithMessage("The appointment date and time must be in the future.")
                                            .When(a => a.AppointmentDate.HasValue);

            RuleFor(a => a.DurationInMinutes).GreaterThanOrEqualTo(10).WithMessage("The inspection should last at least 10 minutes.")
                                             .LessThanOrEqualTo(120).WithMessage("The inspection cannot exceed 120 minutes.")
                                             .When(a => a.DurationInMinutes.HasValue);

            RuleFor(a => a.RoomId).NotEmpty().WithMessage("The provided Room Id is invalid")
                .When(a => a.RoomId.HasValue);

            RuleFor(a => a.PatientId).NotEmpty().WithMessage("The provided Patient Id is invalid")
                .When(a => a.PatientId.HasValue);

            RuleFor(a => a.DoctorId).NotEmpty().WithMessage("The provided Doctor Id is invalid")
                .When(a => a.DoctorId.HasValue);
        }
    }
}
