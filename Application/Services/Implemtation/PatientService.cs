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
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public PatientService(IUnitOfWork unitOfWork, IMapper mapper , UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ReadPatientDto> AddAsync(CreatePatientDto dto)
        {
            var userExists = await _userManager.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
                throw new NotFoundException("The associated user account was not found.");

            var patient = _mapper.Map<Patient>(dto);

            await _unitOfWork.Repository<Patient>().AddAsync(patient);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to save the patient record to the database.");
            
            return _mapper.Map<ReadPatientDto>(patient);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var patientRepo = _unitOfWork.Repository<Patient>();

            var patient = await patientRepo.FindAsync(p => p.Id == Id);
            if (patient is null)
                throw new NotFoundException("No patient found with the provided ID.");

            patientRepo.Delete(patient);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to delete the patient record.");

            return true;
        }

        public async Task<PagedResult<ReadPatientDto>> GetAllAsync(PaginationParams pagination , string? NameOrPhone)
        {
            int skip = (pagination.PageNumber - 1) * pagination.PageSize;
            int take = pagination.PageSize;

            Expression<Func<Patient,bool>> filter = p => string.IsNullOrEmpty(NameOrPhone) || p.Name.Contains(NameOrPhone) || p.PhoneNumber.Contains(NameOrPhone);

            var patientRepo = _unitOfWork.Repository<Patient>();

            var patients = await patientRepo.GetPagedResultAsync(filter, skip, take);

            int totalCount = await patientRepo.CountAsync(filter);

            var patientDto = _mapper.Map<IEnumerable<ReadPatientDto>>(patients);

            return new PagedResult<ReadPatientDto>
            {
                Data = patientDto.ToList(),
                PageNumber = pagination.PageNumber,
                TotalCount = totalCount,
                PageSize = pagination.PageSize
            };
        }

        public async Task<ReadPatientDto> GetByIdAsync(Guid Id)
        {
            var patient = _unitOfWork.Repository<Patient>().GetByIdAsync(Id);
            if (patient is null)
                throw new NotFoundException("No patient found with the provided ID.");

            return _mapper.Map<ReadPatientDto>(patient);
        }


        public async Task<bool> UpdateAsync(Guid id, UpdatePatientDto dto)
        {
            var patientRepo = _unitOfWork.Repository<Patient>();
            var patient = await patientRepo.GetByIdAsync(id);

            if (patient is null)
                throw new NotFoundException("No patient found with the provided ID.");

            patient.Name = dto.Name;
            patient.PhoneNumber = dto.PhoneNumber;
            patient.BloodType = dto.BloodType;
            patient.ChronicDiseases = dto.ChronicDiseases;

            patientRepo.Update(patient);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to update the patient medical profile.");

            return true;
        }
    }
}
