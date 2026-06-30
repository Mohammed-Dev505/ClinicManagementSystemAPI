using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService) => _roomService = roomService;
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]PaginationParams pagination)
        {
            var result = await _roomService.GetAllAsync(pagination);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _roomService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRoomDto dto)
        {
            var result = await _roomService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateRoomDto dto)
        {
            var result = await _roomService.UpdateAsync(id , dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _roomService.DeleteAsync(id);
            return NoContent();
        }
    }
}
