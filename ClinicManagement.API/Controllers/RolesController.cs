using ClinicManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public class CreateRoleDto
        {
            [Required]
            public string RoleName { get; set; }
        }
        public class AssignRoleDto
        {
            [Required]
            public string UserId { get; set; }
            [Required]
            public string RoleName { get; set; }
        }

        public RolesController(UserManager<User> userManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(roles);
        }
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRle(CreateRoleDto model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleName))
                return BadRequest("Role name is required.");

            if (await _roleManager.RoleExistsAsync(model.RoleName))
                return BadRequest("This role already exists in the system.");

            await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
            return Created();
        }
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                return NotFound("The spcified user was not found.");
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
                return BadRequest("The specified role does not exist in the system. Please create the role first.");

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return NoContent();
        }
        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
                return NotFound("The specified role was not found.");
            if (roleName.ToLower() == "admin")
                return BadRequest("The 'Admin' role is a core system role and cannot be deleted.");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return NoContent();
        }
    }
}
