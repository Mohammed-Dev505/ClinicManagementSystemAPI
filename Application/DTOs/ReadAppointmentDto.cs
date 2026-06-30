using ClinicManagement.API.Domain;
using System.Text.Json.Serialization;

namespace ClinicManagement.API.Application.DTOs
{
    public class ReadAppointmentDto
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string SpecialtyName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string RoomNumber { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnAppointmentStatus Status { get; set; }
    }
}
