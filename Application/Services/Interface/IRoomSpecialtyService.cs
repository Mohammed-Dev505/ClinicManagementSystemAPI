using ClinicManagement.API.Application.DTOs;

public interface IRoomSpecialtyService
{
    Task<bool> AssignSpecialtyToRoomAsync(CreateRoomSpecialtyDto dto);

    Task<bool> UnassignSpecialtyFromRoomAsync(Guid roomId, Guid specialtyId);

    Task<bool> UpdateRoomSpecialtyAsync(UpdateRoomSpecialtyDto dto);

    Task<IEnumerable<ReadRoomSpecialtyDto>> GetSpecialtiesByRoomIdAsync(Guid roomId);
}