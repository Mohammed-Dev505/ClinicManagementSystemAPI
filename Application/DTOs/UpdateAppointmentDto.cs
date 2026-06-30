namespace ClinicManagement.API.Application.DTOs
{
    public class UpdateAppointmentDto
    {
        public DateTime? AppointmentDate { get; set; }
        public int? DurationInMinutes { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? RoomId { get; set; }
    }
}
