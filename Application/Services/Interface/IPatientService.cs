using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Services.Interface
{
    public interface IPatientService
    {
        Task<ReadPatientDto> AddAsync(CreatePatientDto dto);
        Task<bool> UpdateAsync(Guid id , UpdatePatientDto dto);
        Task<bool> DeleteAsync(Guid Id);
        Task<ReadPatientDto> GetByIdAsync(Guid Id);
        Task<PagedResult<ReadPatientDto>> GetAllAsync(PaginationParams pagination , string? NameOrPhone);
    }
}
