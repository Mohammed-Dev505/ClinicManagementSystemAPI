using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Services.Interface
{
    public interface IRoomService
    {
        Task<ReadRoomDto> AddAsync(CreateRoomDto dto);
        Task<bool> UpdateAsync(Guid id , UpdateRoomDto dto);
        Task<bool> DeleteAsync(Guid Id);
        Task<ReadRoomDto> GetByIdAsync(Guid Id);
        Task<PagedResult<ReadRoomDto>> GetAllAsync(PaginationParams pagination);

    }
}
