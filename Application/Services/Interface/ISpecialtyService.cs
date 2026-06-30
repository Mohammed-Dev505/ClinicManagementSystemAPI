using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Services.Interface
{
    public interface ISpecialtyService
    {
        Task<ReadSpecialtyDto> AddAsync(CreateSpecialtyDto dto);
        Task<bool> UpdateAsync(Guid id ,  UpdateSpecialtyDto dto);
        Task<bool> DeleteAsync(Guid Id);
        Task<ReadSpecialtyDto> GetByIdAsync(Guid Id);
        Task<PagedResult<ReadSpecialtyDto>> GetAllAsync(PaginationParams pagination);
    }
}
