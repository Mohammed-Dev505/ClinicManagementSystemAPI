

namespace ClinicManagement.API.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationInMinutes { get; set; }
        public EnAppointmentStatus Status { get; set; } = EnAppointmentStatus.Pending;

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
