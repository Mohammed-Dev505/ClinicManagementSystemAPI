using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model , string defaultRole = "Patient");
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<bool> AddRoleAsync(AddRoleModel model);
        Task<bool> RemoveRoleAsync(AddRoleModel model);
        Task<bool> ChangePasswordAsync(ChangePasswordModel model);
    }
}
