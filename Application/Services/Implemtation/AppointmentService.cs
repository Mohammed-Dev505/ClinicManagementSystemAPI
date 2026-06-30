using Application.Services.Interface;
using AutoMapper;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Application.Exceptions;
using ClinicManagement.API.Domain;
using ClinicManagement.API.Domain.Entities;
using ClinicManagement.API.Services.Interface;

namespace ClinicManagement.API.Application.Services.Implemtation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReadAppointmentDto> AddAsync(CreateAppointmentDto dto)
        {
            dto.AppointmentDate = new DateTime(dto.AppointmentDate.Year, dto.AppointmentDate.Month, dto.AppointmentDate.Day, dto.AppointmentDate.Hour, dto.AppointmentDate.Minute, 0);

            var pateintExists =  await _unitOfWork.Repository<Patient>().AnyAsync(p => p.Id == dto.PatientId);
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(dto.DoctorId);
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(dto.RoomId);

            if (!pateintExists)
                throw new NotFoundException("The speciafied patient was not found.");
            if (doctor is null)
                throw new NotFoundException("The specified doctor was not found.");
            if (room is null)
                throw new NotFoundException("The specified room was not found.");

            var isRoomSubitable = await _unitOfWork.Repository<RoomSpecialty>().AnyAsync(rs => rs.RoomId == room.Id && rs.SpecialtyId == doctor.SpecialtyId);
            if (!isRoomSubitable)
                throw new BadRequestException("This room isnot equipped or designated for the doctor's specialty.");

            if (!await IsRoomAvailableAsync(room.Id, dto.AppointmentDate, dto.DurationInMinutes, null))
                throw new BadRequestException("The selected room is not available at this date and time.");

            if (!await IsDoctorAvailableAsync(doctor.Id, dto.AppointmentDate, dto.DurationInMinutes, null))
                throw new BadRequestException("The selected doctor is not available at this date and time.");

            var appointment = _mapper.Map<Appointment>(dto);

            await _unitOfWork.Repository<Appointment>().AddAsync(appointment);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to create the appointment.");

            return _mapper.Map<ReadAppointmentDto>(appointment);
        }

        public async Task<IEnumerable<ReadAppointmentDto>> GetAllAsync()
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetAllAsync();
            return _mapper.Map<IEnumerable<ReadAppointmentDto>>(appointment);
        }

        public async Task<ReadAppointmentDto> GetByIdAsync(Guid Id)
        {
           var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(Id);
            if (appointment is null)
                throw new NotFoundException("No appointment found with the provided ID.");

            return _mapper.Map<ReadAppointmentDto>(appointment);
        }

        public async Task<bool> UpdateStatusAsync(Guid Id, EnAppointmentStatus status)
        {
            var appointmentRepo = _unitOfWork.Repository<Appointment>();
            var appointment = await appointmentRepo.GetByIdAsync(Id);

            if (appointment is null)
                throw new NotFoundException("The appointment was not found.");

            if(appointment.Status == EnAppointmentStatus.Canceled)
            {
                throw new BadRequestException("Cannot change status, The appointment is already cancelled.");
            }
            if(appointment.Status == EnAppointmentStatus.Completed)
            {
                throw new BadRequestException("Cannot change status, the appointment is already completed.");
            }
            appointment.Status = status;

            appointmentRepo.Update(appointment);

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Faield toupdate the appointment status.");
            return true;
        }
        public async Task<bool> RescheduleAppointmentAsync(Guid Id , UpdateAppointmentDto dto)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(Id);

            if (appointment is null)
                throw new NotFoundException("The appointment was not found.");

            if (dto.AppointmentDate != null && dto.AppointmentDate.Value != appointment.AppointmentDate)
            {
                var newDate = dto.AppointmentDate.Value;
                appointment.AppointmentDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, newDate.Hour, newDate.Minute, 0);
            }
            if(dto.DurationInMinutes != null  && dto.DurationInMinutes.Value != appointment.DurationInMinutes)
            {
                appointment.DurationInMinutes = dto.DurationInMinutes.Value;
            }
            if (dto.DoctorId != null && dto.DoctorId.Value != appointment.DoctorId)
            {
                var doctorExists = await _unitOfWork.Repository<Doctor>().AnyAsync(a => a.Id == dto.DoctorId);
                if (!doctorExists) throw new NotFoundException("The specified doctor was not found.");
                appointment.DoctorId = dto.DoctorId.Value;
            }
            if(dto.PatientId != null && dto.PatientId.Value != appointment.PatientId)
            {
                var patientExists = await _unitOfWork.Repository<Patient>().AnyAsync(a => a.Id == dto.PatientId);
                if (!patientExists) throw new NotFoundException("The specified patient was not found.");
                appointment.PatientId = dto.PatientId.Value;
            }
            if(dto.RoomId != null && dto.RoomId.Value != appointment.RoomId)
            {
                var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(a => a.Id == dto.RoomId);
                if (!roomExists) throw new NotFoundException("The specified room was not found.");
                appointment.RoomId = dto.RoomId.Value;
            }

            var checkDoctor = await IsDoctorAvailableAsync(appointment.DoctorId, appointment.AppointmentDate, appointment.DurationInMinutes, appointment.Id);
            if (!checkDoctor) throw new BadRequestException("The doctor is not available at the newly requested date and time.");

            var checkRoom = await IsRoomAvailableAsync(appointment.RoomId, appointment.AppointmentDate, appointment.DurationInMinutes, appointment.Id);
            if (!checkRoom) throw new BadRequestException("The room is not available at the newly requested date and time.");

            var saveResult = await _unitOfWork.CompleteAsync();
            if (saveResult < 1)
                throw new BadRequestException("Failed to reschedule the appointment.");
            return true;
        }
        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime dateTime, int durationInMinutes = 0, Guid? appointmentId = null)
        {
            var StartDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            var EndDate = StartDate.AddMinutes(durationInMinutes);
            if (appointmentId is null)
            {
                var roomExists = await _unitOfWork.Repository<Room>().AnyAsync(a => a.Id == roomId);
                if (!roomExists) throw new NotFoundException("The specified room was not found");
                var hasConflict = await _unitOfWork.Repository<Appointment>()
                                 .AnyAsync(a => a.RoomId == roomId && a.AppointmentDate < EndDate && a.AppointmentDate.AddMinutes(a.DurationInMinutes) > StartDate);
                return !hasConflict;
            }
            else
            {
                var hasConflict = await _unitOfWork.Repository<Appointment>()
                                   .AnyAsync(a => a.Id != appointmentId && a.RoomId == roomId && a.AppointmentDate < EndDate && a.AppointmentDate.AddMinutes(a.DurationInMinutes) > StartDate);
                return !hasConflict;
            }
        }
        public async Task<bool> IsDoctorAvailableAsync(Guid doctorId , DateTime dateTime, int durationInMinutes = 0, Guid? appointmentId = null)
        {
            var StartDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            var EndDate = StartDate.AddMinutes(durationInMinutes);
            if(appointmentId is null)
            {
                var doctorExists = await _unitOfWork.Repository<Doctor>().AnyAsync(a => a.Id == doctorId);
                if (!doctorExists) throw new NotFoundException("The specified doctor was not found");
                var hasCobflict = await _unitOfWork.Repository<Appointment>().
                                 AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate < EndDate && a.AppointmentDate.AddMinutes(a.DurationInMinutes) > StartDate);
                return !hasCobflict;
            }
            else
            {
                var hasCobflict = await _unitOfWork.Repository<Appointment>().
                                AnyAsync(a => a.Id != appointmentId && a.DoctorId == doctorId && a.AppointmentDate < EndDate && a.AppointmentDate.AddMinutes(a.DurationInMinutes) > StartDate);
                return !hasCobflict;
            }
        }
    }
}
