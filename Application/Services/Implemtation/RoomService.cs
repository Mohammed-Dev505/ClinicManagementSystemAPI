using Application.Services.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Exceptions;
using ClinicManagement.API.Application.Models;
using ClinicManagement.API.Domain.Entities;
using ClinicManagement.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Application.Services.Implemtation
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReadRoomDto> AddAsync(CreateRoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);

            await _unitOfWork.Repository<Room>().AddAsync(room);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to create the room record.");

            return _mapper.Map<ReadRoomDto>(room);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var roomRepo = _unitOfWork.Repository<Room>();
            var room = await roomRepo.GetByIdAsync(Id);
            if (room is null)
                throw new NotFoundException("No room found with the provided ID.");

            roomRepo.Delete(room);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to delete the room record.");

            return true;
        }

        public async Task<PagedResult<ReadRoomDto>> GetAllAsync(PaginationParams pagination)
        {
            int skip = (pagination.PageNumber - 1 ) * pagination.PageSize;
            int take = pagination.PageSize;

            var roomRepo = _unitOfWork.Repository<Room>();

            var rooms = await roomRepo.GetPagedResultAsync(null, skip, take);

            int totalCount = await roomRepo.CountAsync(null);

            var roomDto = _mapper.Map<IEnumerable<ReadRoomDto>>(rooms);

            return new PagedResult<ReadRoomDto>
            {
                Data = roomDto.ToList(),
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount
            };

        }

        public async Task<ReadRoomDto> GetByIdAsync(Guid Id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(Id);
            if (room is null)
                throw new NotFoundException("No room found with the provided ID.");

            return _mapper.Map<ReadRoomDto>(room);
        }


        public async Task<bool> UpdateAsync(Guid id, UpdateRoomDto dto)
        {
            var roomRepo = _unitOfWork.Repository<Room>();

            var room = await roomRepo.GetByIdAsync(id);
            if (room is null)
                throw new NotFoundException("No room found with the provided ID.");

            room.RoomNumber = dto.RoomNumber;

            roomRepo.Update(room);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to update the room details.");
            return true;
        }
    }
}
