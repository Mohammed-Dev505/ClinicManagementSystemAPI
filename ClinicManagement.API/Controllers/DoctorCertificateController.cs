using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagement.API.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorCertificateController : ControllerBase
    {
        private readonly IDoctorCertificateService _doctorCertificateService;
        public DoctorCertificateController(IDoctorCertificateService doctorCertificateService) => _doctorCertificateService = doctorCertificateService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var result = await _doctorCertificateService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorsCertificates([FromRoute] Guid doctorId)
        {
            var result = await _doctorCertificateService.GetDoctorsCertificatesAsync(doctorId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateDoctorCertificateDto dto)
        {
            var result = await _doctorCertificateService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateDoctorCertificateDto dto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            await _doctorCertificateService.UpdateAsync(id, dto, currentUserId, isAdmin);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isAdmin = User.IsInRole("Admin");
            await _doctorCertificateService.DeleteAsync(id , currentUserId , isAdmin);
            return NoContent();
        }
    }
}
