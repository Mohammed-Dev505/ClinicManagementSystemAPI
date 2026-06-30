using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.API.Domain.Entities
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<RoomSpecialty> RoomSpecialties { get; set; }
    }
}
