using ClinicManagement.API.Application.Services.Implemtation;
using ClinicManagement.API.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IRoomSpecialtyService, RoomSpecialtyService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IDoctorCertificateService, DoctorCertificateService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
