namespace ClinicManagement.API.Application.DTOs
{
    public class CreateRoomSpecialtyDto
    {
        public Guid roomId { get; set; }
        public Guid specialtyId { get; set; }
    }
}
