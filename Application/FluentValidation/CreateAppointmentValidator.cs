using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(p => p.PatientId).NotEmpty().WithMessage("The provided Patient Id is invalid").NotNull();
            
            RuleFor(p => p.DoctorId).NotEmpty().WithMessage("The provided Doctor Id is invalid").NotNull();

            RuleFor(p => p.RoomId).NotEmpty().WithMessage("The provided Room Id is invalid").NotNull();

            RuleFor(p => p.AppointmentDate).NotEmpty().WithMessage("Appointment date id is required")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("The appointment date must be in the future").NotNull();

            RuleFor(d => d.DurationInMinutes).NotEmpty().WithMessage("DurationInMinutes is required")
                                             .GreaterThanOrEqualTo(10).WithMessage("The inspection should last at least 10 minutes")
                                             .LessThanOrEqualTo(120).WithMessage("The inspection cannot exceed 120 minutes").NotNull();
        }
    }
}
