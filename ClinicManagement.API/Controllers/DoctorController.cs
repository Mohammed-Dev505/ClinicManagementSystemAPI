using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination , [FromQuery]string? specialty)
        {
            var doctors = await _doctorService.GetAllAsync(pagination , specialty);
            return Ok(doctors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            return Ok(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]CreateDoctorDto dto)
        {
            var result = await _doctorService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateDoctorDto dto)
        {
            var result = await _doctorService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _doctorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
