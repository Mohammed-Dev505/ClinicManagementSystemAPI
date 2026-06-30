using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.API.Domain.Entities
{
    public class Specialty
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<RoomSpecialty> RoomSpecialties { get; set; }
    }
}
