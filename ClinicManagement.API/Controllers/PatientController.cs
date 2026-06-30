using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Models;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
    [Authorize(Roles = "Admin,Rexeptionist")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService) => _patientService = patientService;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination  , [FromQuery] string? NameOrPhone)
        {
            var result = await _patientService.GetAllAsync(pagination, NameOrPhone);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _patientService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePatientDto dto)
        {
            var result = await _patientService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , UpdatePatientDto dto)
        {
            var result = await _patientService.UpdateAsync(id , dto);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await  _patientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
