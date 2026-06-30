using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]PaginationParams pagination)
        {
            var specialties = await _specialtyService.GetAllAsync(pagination);
            return Ok(specialties);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);
            return Ok(specialty);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSpecialtyDto dto)
        {
            var result = await _specialtyService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateSpecialtyDto dto)
        {
            var result = await _specialtyService.UpdateAsync(id , dto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var result = await _specialtyService.DeleteAsync(id);
            return NoContent(); 
        }
    }
}
