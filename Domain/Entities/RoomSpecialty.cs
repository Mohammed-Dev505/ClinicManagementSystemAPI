namespace ClinicManagement.API.Domain.Entities
{
    public class RoomSpecialty
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
    }
}
