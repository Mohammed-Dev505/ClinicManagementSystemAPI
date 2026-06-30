using Application.Services.Interface;
using AutoMapper;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Exceptions;
using ClinicManagement.API.Application.Models;
using ClinicManagement.API.Domain.Entities;
using ClinicManagement.API.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicManagement.API.Application.Services.Implemtation
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper , UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ReadDoctorDto> AddAsync(CreateDoctorDto dto)
        {
            var userExists = await _userManager.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
                throw new NotFoundException("The associated user account was not found");

            var doctor = _mapper.Map<Doctor>(dto);

            await _unitOfWork.Repository<Doctor>().AddAsync(doctor);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to save the doctor record to the database.");


            return _mapper.Map<ReadDoctorDto>(doctor);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var doctor = await doctorRepo.FindAsync(d => d.Id ==  Id);

            if (doctor is null)
                throw new NotFoundException("No doctor found with the provided ID.");

            doctorRepo.Delete(doctor);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to delete the doctor record.");

            return true;
        }

        public async Task<PagedResult<ReadDoctorDto>> GetAllAsync(PaginationParams pagination , string? specialty = null )
        {
            int skip = (pagination.PageNumber - 1) * pagination.PageSize;
            int take = pagination.PageSize;

            Expression<Func<Doctor, bool>> filter = d => string.IsNullOrEmpty(specialty) || d.Specialty.Name == specialty;

            var doctorRepo = _unitOfWork.Repository<Doctor>();

            var doctors = await doctorRepo.GetPagedResultAsync(filter, skip, take);
            int totalCount = await doctorRepo.CountAsync(filter);

            var doctorDto = _mapper.Map<IEnumerable<ReadDoctorDto>>(doctors);

            return new PagedResult<ReadDoctorDto>
            {
                Data = doctorDto.ToList(),
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
            };
        }

        public async Task<ReadDoctorDto> GetByIdAsync(Guid Id)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(Id);
            if (doctor is null)
                throw new NotFoundException("No doctor found with the provided ID.");

            return _mapper.Map<ReadDoctorDto>(doctor);
        }

        public async Task<bool> UpdateAsync(Guid id , UpdateDoctorDto dto)
        {
            var doctorRepo = _unitOfWork.Repository<Doctor>();

            var doctor = await doctorRepo.GetByIdAsync(id);
            if (doctor is null)
                throw new NotFoundException("No doctor found with the provided ID.");

            doctor.Name = dto.Name;
            doctor.PhoneNumber = dto.PhoneNumber;

            doctorRepo.Update(doctor);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to update the doctor profile.");

            return true;
        }
    }
}
