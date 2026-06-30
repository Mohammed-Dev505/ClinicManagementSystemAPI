using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Domain;

namespace ClinicManagement.API.Services.Interface
{
    public interface IAppointmentService
    {
        Task<ReadAppointmentDto> AddAsync(CreateAppointmentDto dto);
        Task<ReadAppointmentDto> GetByIdAsync(Guid Id);
        Task<IEnumerable<ReadAppointmentDto>> GetAllAsync();
        Task<bool> UpdateStatusAsync(Guid Id, EnAppointmentStatus status);
        Task<bool> RescheduleAppointmentAsync(Guid Id , UpdateAppointmentDto dto);
        Task<bool> IsDoctorAvailableAsync(Guid doctorId, DateTime dateTime, int durationInMinutes = 0, Guid? appointmentId = null);
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime dateTime, int durationInMinutes = 0, Guid? appointmentId = null);
    }
}
