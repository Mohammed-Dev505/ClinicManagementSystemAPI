using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Services.Interface
{
    public interface IDoctorService
    {
        Task<ReadDoctorDto> AddAsync(CreateDoctorDto dto);
        Task<bool> UpdateAsync(Guid id, UpdateDoctorDto dto);
        Task<bool> DeleteAsync(Guid Id);
        Task<ReadDoctorDto> GetByIdAsync(Guid Id);
        Task<PagedResult<ReadDoctorDto>> GetAllAsync(PaginationParams pagination , string? specialty = null);
    }
}
