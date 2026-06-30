using ClinicManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleDto model)
        {
            if (string.IsNullOrEmpty(model.RoleName))
                return BadRequest("اسم الدور مطلوب");

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (roleExists)
                return BadRequest("هذا الدور موجود مسبقاً");

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"بنجاح {model.RoleName}تم انشاء الدور ");
        }
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound("المستخدم غير موجود");
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
                return BadRequest("الدور المكتوب غير موجود بالنظام , يجب انشاؤه اولاً");

            var result = await _userManager.AddToRoleAsync(user,model.RoleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"بنجاح {model.RoleName} تم منح المستخدم دور ");
        }
        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound("هذا الدور غير موجود بالاصل ليتم حذفه");

            if (roleName.ToLower() == "admin" || roleName.ToLower() == "patient")
                return BadRequest("لا يمكن حذف الادوار الاساسية");

            var result = await _roleManager.DeleteAsync(role);
            if(!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"نائياً من النظام {roleName} تم حذف الدور ");
        }
    }
}
