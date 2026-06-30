using ClinicManagement.API.Application.Models;
using ClinicManagement.API.Application.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.API.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }
        [HttpPost("getToken")]
        public async Task<IActionResult> GetToken(TokenRequestModel model)
        {
            var result = await _authService.GetTokenAsync(model);
            return Ok(result);
        }
    }
}
