using Application.Services.Interface;
using AutoMapper;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Exceptions;
using ClinicManagement.API.Application.Models;
using ClinicManagement.API.Domain.Entities;
using ClinicManagement.API.Services.Interface;

namespace ClinicManagement.API.Application.Services.Implemtation
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SpecialtyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReadSpecialtyDto> AddAsync(CreateSpecialtyDto dto)
        {
            var specialty = _mapper.Map<Specialty>(dto);

            await _unitOfWork.Repository<Specialty>().AddAsync(specialty);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to create the medical specialty record.");

            return _mapper.Map<ReadSpecialtyDto>(specialty);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var specialtyRepo = _unitOfWork.Repository<Specialty>();

            var specialty = await specialtyRepo.FindAsync(s => s.Id == Id);

            if (specialty is null)
                throw new NotFoundException("No medical specialty found with the provided ID.");

            specialtyRepo.Delete(specialty);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to delete the medical specialty record.");

            return true;
        }

        public async Task<PagedResult<ReadSpecialtyDto>> GetAllAsync(PaginationParams pagination)
        {
            int skip = (pagination.PageNumber - 1) * pagination.PageSize;
            int take = pagination.PageSize;

            var specialtyRepo = _unitOfWork.Repository<Specialty>();
            var specialtys = await specialtyRepo.GetPagedResultAsync(null, skip, take);

            int totalCount = await specialtyRepo.CountAsync(null);

            return new PagedResult<ReadSpecialtyDto>
            {
                Data = _mapper.Map<IEnumerable<ReadSpecialtyDto>>(specialtys.ToList()),
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount
            };
        
        }

        public async Task<ReadSpecialtyDto> GetByIdAsync(Guid Id)
        {
            var specialty = await _unitOfWork.Repository<Specialty>().FindAsync(a => a.Id == Id);
            if (specialty is null) throw new NotFoundException("No medical specialty found with the provided ID.");
            return _mapper.Map<ReadSpecialtyDto>(specialty);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateSpecialtyDto dto)
        {
            var specialtyRepo = _unitOfWork.Repository<Specialty>();
            var specialty = await specialtyRepo.FindAsync(s => s.Id == id);
            if (specialty is null)
                throw new NotFoundException("No medical specialty found with the provided ID.");
            specialty.Name = dto.Name;

            specialtyRepo.Update(specialty);
            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to update the medical specialty details.");
            return true;
        }
    }
}
