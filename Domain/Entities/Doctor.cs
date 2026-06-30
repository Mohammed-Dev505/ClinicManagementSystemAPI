using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.API.Domain.Entities
{
    public class Doctor
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalLicense { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; }
    }
}
