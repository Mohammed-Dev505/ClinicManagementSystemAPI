namespace ClinicManagement.API.Application.DTOs
{
    public class UpdateRoomSpecialtyDto
    {
        public Guid roomId { get; set; }
        public Guid OldSpecialtyId { get; set; }
        public Guid NewSpecialtyId { get; set; }
    }
}
