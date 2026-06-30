using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Domain;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Authorize(Roles = "Admin,Rexeptionist")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService) => _appointmentService = appointmentService;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _appointmentService.GetAllAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Rexeptionist,Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _appointmentService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpGet("rooms/availability")]
        public async Task<IActionResult> IsRoomAvailable(Guid roomId, DateTime dateTime,  int durationInMinutes = 0)
        {
            var result = await _appointmentService.IsRoomAvailableAsync(roomId, dateTime, durationInMinutes);
            return Ok(result);  
        }
        [HttpGet("doctors/availability")]
        public async Task<IActionResult> IsDoctorAvailable(Guid doctorId, DateTime dateTime, int durationInMinutes = 0)
        {
            var result = await _appointmentService.IsDoctorAvailableAsync(doctorId, dateTime, durationInMinutes);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAppointmentDto dto)
        {
            var result = await _appointmentService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] EnAppointmentStatus status)
        {
            var result = await _appointmentService.UpdateStatusAsync(id, status);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDate([FromRoute]Guid id,[FromBody] UpdateAppointmentDto dto)
        {
            var result = await _appointmentService.RescheduleAppointmentAsync(id, dto);
            return Ok(result);
        }

    }
}
