using ClinicManagement.API.Application.Services.Implemtation;
using ClinicManagement.API.Application.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class IdentityServicesRegisteration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
