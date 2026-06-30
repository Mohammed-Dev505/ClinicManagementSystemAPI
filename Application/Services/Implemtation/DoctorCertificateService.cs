using Application.Services.Interface;
using AutoMapper;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Domain.Entities;
using ClinicManagement.API.Services.Interface;
using SendGrid.Helpers.Errors.Model;
using BadRequestException = ClinicManagement.API.Application.Exceptions.BadRequestException;
using NotFoundException = ClinicManagement.API.Application.Exceptions.NotFoundException;


namespace ClinicManagement.API.Application.Services.Implemtation
{
    public class DoctorCertificateService : IDoctorCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DoctorCertificateService(IUnitOfWork unitOfWork  , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReadDoctorCertificateDto> AddAsync(CreateDoctorCertificateDto dto)
        {
            var doctorExists = await _unitOfWork.Repository<Doctor>().AnyAsync(d => d.Id == dto.DoctorId);

            if (!doctorExists)
                throw new NotFoundException("The specified doctor was not found.");

            var certificateRepo = _unitOfWork.Repository<DoctorCertificate>();

            var ceritifacteExists = await certificateRepo.AnyAsync(a => a.DoctorId == dto.DoctorId &&
                                                                                                   a.CertificateName.ToLower() == dto.CertificateName.ToLower() &&
                                                                                                   a.IssuingOrganization.ToLower() == dto.IssuingOrganization.ToLower());
            if (ceritifacteExists)
                throw new BadRequestException("This certificate from the specified issunig organization has already been added for this doctor.");

            var certificate = _mapper.Map<DoctorCertificate>(dto);

            await certificateRepo.AddAsync(certificate);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to save the doctor certificate.");

            return _mapper.Map<ReadDoctorCertificateDto>(certificate);
        }

        public async Task<bool> DeleteAsync(Guid id , string currentUserId , bool isAdmin)
        {
            var certificateRepo = _unitOfWork.Repository<DoctorCertificate>();
            var certificate = await certificateRepo.FindAsync(c => c.Id == id);

            if (certificate is null)
                throw new NotFoundException("No certificate found with provided ID.");

            if (certificate.DoctorId.ToString() != currentUserId && !isAdmin)
                throw new ForbiddenException("You don't have premission to delete this certificate.");

            certificateRepo.Delete(certificate);

            var savecResult = await _unitOfWork.CompleteAsync();
            if (savecResult < 1)
                throw new BadRequestException("Failed to delete the doctor certificate.");

            return true;
        }

        public async Task<ReadDoctorCertificateDto> GetByIdAsync(Guid id)
        {
            var certificate = await _unitOfWork.Repository<DoctorCertificate>().GetByIdAsync(id);
            if (certificate is null)
                throw new NotFoundException("No certificate found with provided ID.");
            return _mapper.Map<ReadDoctorCertificateDto>(certificate);
        }

        public async Task<IEnumerable<ReadDoctorCertificateDto>> GetDoctorsCertificatesAsync(Guid doctorId)
        {
            var doctorExists = await _unitOfWork.Repository<Doctor>().AnyAsync(d => d.Id ==  doctorId);
            if (!doctorExists)
                throw new NotFoundException("The specified doctor was not found.");
            var doctorCertificates = await _unitOfWork.Repository<DoctorCertificate>().FindAsync(a => a.DoctorId == doctorId);
            return _mapper.Map<IEnumerable<ReadDoctorCertificateDto>>(doctorCertificates);
        }

        public async Task<bool> UpdateAsync(Guid Id, UpdateDoctorCertificateDto dto , string currentUserId , bool isAdmin)
        {
            var certificateRepo = _unitOfWork.Repository<DoctorCertificate>();
            var certificate = await certificateRepo.GetByIdAsync(Id);

            if (certificate is null)
                throw new NotFoundException("No certificate found with the provided ID.");

            if(!isAdmin && certificate.DoctorId.ToString() != currentUserId)
            {
                throw new ForbiddenException("You don't have permission to update this certificate.");
            }

            bool isDuplicate = await certificateRepo.AnyAsync(c =>
            c.CertificateName.ToLower() == dto.CertificateName.ToLower() &&
            c.DoctorId.ToString() == currentUserId &&
            c.Id != Id);

            if(isDuplicate)
            {
                throw new BadRequestException("A certificate wuth this name has already been added for this doctor.");
            }

            certificate.CertificateName = dto.CertificateName ?? certificate.CertificateName;
            certificate.IssuingOrganization = dto.IssuingOrganization ?? certificate.IssuingOrganization;

            certificateRepo.Update(certificate);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
    }
}
