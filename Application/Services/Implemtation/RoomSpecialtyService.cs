using Application.Services.Interface;
using AutoMapper;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Exceptions;
using ClinicManagement.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Application.Services.Implemtation
{
    public class RoomSpecialtyService : IRoomSpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomSpecialtyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> AssignSpecialtyToRoomAsync(CreateRoomSpecialtyDto dto)
        {
            var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(a => a.Id == dto.roomId);
            if (!roomExists)
                throw new NotFoundException("The specified room was not found");
            var specialtyExists = await _unitOfWork.Repository<Specialty>().AnyAsync(a => a.Id == dto.specialtyId);
            if (!specialtyExists)
                throw new NotFoundException("The specialty is not found");
            var roomSpecialtyExists = await _unitOfWork.Repository<RoomSpecialty>().AnyAsync(a => a.RoomId == dto.roomId &&  a.SpecialtyId == dto.specialtyId);
            if (roomSpecialtyExists)
                throw new BadRequestException("The specified specialty is already assigned to this room");
            var rspecialty = new RoomSpecialty
            {
                RoomId = dto.roomId,
                SpecialtyId = dto.specialtyId,
            };
            await _unitOfWork.Repository<RoomSpecialty>().AddAsync(rspecialty);
            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to assign the specialty to the room.");
            return true;
        }

        public async Task<IEnumerable<ReadRoomSpecialtyDto>> GetSpecialtiesByRoomIdAsync(Guid roomId)
        {
            var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(a => a.Id ==  roomId);
            if (!roomExists)
                throw new NotFoundException("The specified room was not found");
            var specialties = await _unitOfWork.Repository<RoomSpecialty>().FindAsync(r => r.RoomId == roomId);
            return _mapper.Map<IEnumerable<ReadRoomSpecialtyDto>>(specialties);
        }

        public async Task<bool> UnassignSpecialtyFromRoomAsync(Guid roomId, Guid specialtyId)
        {
            var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(a => a.Id == roomId);

            if (!roomExists)
                throw new NotFoundException("The specified room was not found");

            var specialty = await _unitOfWork.Repository<Specialty>().AnyAsync(a => a.Id == specialtyId);

            if (!specialty)
                throw new NotFoundException("The specialty is not found");

            var roomspecialty = await _unitOfWork.Repository<RoomSpecialty>().FindAsync(a => a.RoomId == roomId && a.SpecialtyId == specialtyId);

            if (roomspecialty is null)
                throw new NotFoundException("The specified specialty is not assigned to this room.");

            _unitOfWork.Repository<RoomSpecialty>().Delete(roomspecialty);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to unassign the specialty from the room.");

            return true;
        }

        public async Task<bool> UpdateRoomSpecialtyAsync(UpdateRoomSpecialtyDto dto)
        {
            var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(a => a.Id == dto.roomId);

            if (!roomExists)
                throw new NotFoundException("The specified room was not found.");

            var newSpecialtyExists = await _unitOfWork.Repository<Specialty>().AnyAsync(a => a.Id == dto.NewSpecialtyId);

            if (!newSpecialtyExists)
                throw new NotFoundException("The newly specified specialty was not found.");

            var oldRoomSpecialty = await _unitOfWork.Repository<RoomSpecialty>().FindAsync(a => a.RoomId == dto.roomId && a.SpecialtyId == dto.OldSpecialtyId);

            if (oldRoomSpecialty is null)
                throw new NotFoundException("The specified old specialty is not assigned to this room.");

            var isNewAlreadyAssigned = await _unitOfWork.Repository<RoomSpecialty>().AnyAsync(a => a.RoomId == dto.roomId && a.SpecialtyId == dto.NewSpecialtyId);

            if (isNewAlreadyAssigned)
                throw new BadRequestException("The newly pecified specialty is already assined to this room.");

            _unitOfWork.Repository<RoomSpecialty>().Delete(oldRoomSpecialty);

            var newRoomSpecialty = new RoomSpecialty
            {
                RoomId = dto.roomId,
                SpecialtyId = dto.NewSpecialtyId
            };

            await _unitOfWork.Repository<RoomSpecialty>().AddAsync(newRoomSpecialty);
            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to update the room specialty confiduration.");
            return true;
        }
    }
}
