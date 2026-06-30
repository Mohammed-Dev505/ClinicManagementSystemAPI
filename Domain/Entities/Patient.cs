using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.API.Domain.Entities
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } 
        public string BloodType { get; set; } = string.Empty;
        public string ChronicDiseases { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; }
    }
}
