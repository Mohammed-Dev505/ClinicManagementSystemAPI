using ClinicManagement.API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomSpecialtyController : ControllerBase
    {
        private readonly IRoomSpecialtyService _roomSpecialtyService;
        public RoomSpecialtyController(IRoomSpecialtyService roomSpecialtyService)
        {
            _roomSpecialtyService = roomSpecialtyService;
        }
        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetByRoom(Guid roomId)
        {
            var result = await _roomSpecialtyService.GetSpecialtiesByRoomIdAsync(roomId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AssignSpecialty([FromQuery]CreateRoomSpecialtyDto dto)
        {
            var result = await _roomSpecialtyService.AssignSpecialtyToRoomAsync(dto);
            return Created();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoomSpecialtyDto dto)
        {
            var result = await _roomSpecialtyService.UpdateRoomSpecialtyAsync(dto);
            return NoContent();
        }
        [HttpDelete("room/{roomId}/specialty/{specialtyId}")]
        public async Task<IActionResult> Delete(Guid roomId, Guid specialtyId)
        {
            var result = await _roomSpecialtyService.UnassignSpecialtyFromRoomAsync(roomId, specialtyId);
            return NoContent();
        }

    }
}
