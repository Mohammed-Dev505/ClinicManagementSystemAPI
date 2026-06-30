using ClinicManagement.API.Application.DTOs;

namespace ClinicManagement.API.Services.Interface
{
    public interface IDoctorCertificateService
    {
        Task<ReadDoctorCertificateDto> AddAsync(CreateDoctorCertificateDto dto);
        Task<bool> UpdateAsync(Guid Id ,  UpdateDoctorCertificateDto dto, string currentUserId, bool isAdmin);
        Task<bool> DeleteAsync(Guid id, string currentUserId, bool isAdmin);
        Task<ReadDoctorCertificateDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ReadDoctorCertificateDto>> GetDoctorsCertificatesAsync(Guid doctorId);
    }
}
