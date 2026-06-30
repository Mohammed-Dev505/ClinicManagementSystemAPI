using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.API.Application.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
