using AutoMapper;
using ClinicManagement.API.Application.DTOs;
using ClinicManagement.API.Domain.Entities;

namespace ClinicManagement.API.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping for Appointment

            CreateMap<Appointment, CreateAppointmentDto>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(opt => opt.PatientId)).ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate)).ReverseMap();

            CreateMap<Appointment, ReadAppointmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name))
                .ForMember(dest => dest.SpecialtyName, opt => opt.MapFrom(src => src.Doctor.Specialty.Name))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Appointment, UpdateAppointmentDto>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.DurationInMinutes))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId)).ReverseMap();

            // Mapping for Doctor

            CreateMap<Doctor, CreateDoctorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.SpecialtyId, opt => opt.MapFrom(src => src.SpecialtyId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Medicallicense, opt => opt.MapFrom(src => src.MedicalLicense))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography)).ReverseMap();

            CreateMap<Doctor, ReadDoctorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.SpecialtyName, opt => opt.MapFrom(src => src.Specialty.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.MedicalLicense, opt => opt.MapFrom(src => src.MedicalLicense))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography));

            CreateMap<Doctor, UpdateDoctorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.MedicalLicense, opt => opt.MapFrom(src => src.MedicalLicense))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
                .ForMember(dest => dest.SpecialtyId, opt => opt.MapFrom(src => src.SpecialtyId)).ReverseMap();

            // Mapping for Patient

            CreateMap<Patient, CreatePatientDto>()
                .ForMember(dest => dest.UserId , opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.BloodType))
                .ForMember(dest => dest.ChronicDiseases, opt => opt.MapFrom(src => src.ChronicDiseases))
                .ReverseMap();

            CreateMap<Patient, ReadPatientDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.BloodType))
                .ForMember(dest => dest.ChronicDiseases, opt => opt.MapFrom(src => src.ChronicDiseases));

            CreateMap<Patient, UpdatePatientDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.BloodType))
                .ForMember(dest => dest.ChronicDiseases, opt => opt.MapFrom(src => src.ChronicDiseases))
                .ReverseMap();

            // Mapping for Room

            CreateMap<Room , CreateRoomDto>().ForMember(dest => dest.RoomNumber ,opt => opt.MapFrom(src => src.RoomNumber)).ReverseMap();

            CreateMap<Room, ReadRoomDto>().ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber));

            CreateMap<Room, UpdateRoomDto>().ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber)).ReverseMap();

            // Mapping for Specialty

            CreateMap<Specialty, CreateSpecialtyDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();

            CreateMap<Specialty, ReadSpecialtyDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Specialty, UpdateSpecialtyDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();

            // Mapping for RoomSpecialty

            CreateMap<RoomSpecialty, CreateRoomSpecialtyDto>().ForMember(dest => dest.roomId, opt => opt.MapFrom(src => src.RoomId))
                                                            .ForMember(dest => dest.specialtyId, opt => opt.MapFrom(src => src.SpecialtyId)).ReverseMap();
            CreateMap<RoomSpecialty, UpdateRoomSpecialtyDto>().ForMember(dest => dest.roomId, opt => opt.MapFrom(src => src.RoomId))
                                                                .ForMember(dest => dest.roomId, opt => opt.MapFrom(src => src.SpecialtyId)).ReverseMap();
            CreateMap<RoomSpecialty, ReadRoomSpecialtyDto>().ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                                                             .ForMember(dest => dest.SpecialtyName, opt => opt.MapFrom(src => src.Specialty.Name));

            // Mapping for DoctorCertificate

            CreateMap<DoctorCertificate, CreateDoctorCertificateDto>().ForMember(dest => dest.CertificateName, opt => opt.MapFrom(src => src.CertificateName))
                .ForMember(dest => dest.IssuingOrganization, opt => opt.MapFrom(src => src.IssuingOrganization))
                .ForMember(dest => dest.GraduationDate, opt => opt.MapFrom(src => src.GraduationDate))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId)).ReverseMap();

            CreateMap<DoctorCertificate, ReadDoctorCertificateDto>().ForMember(dest => dest.CertificateName, opt => opt.MapFrom(src => src.CertificateName))
                .ForMember(dest => dest.IssuingOrganization, opt => opt.MapFrom(src => src.IssuingOrganization))
                .ForMember(dest => dest.GraduationDate, opt => opt.MapFrom(src => src.GraduationDate))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId));

            CreateMap<DoctorCertificate, UpdateDoctorCertificateDto>().ForMember(dest => dest.CertificateName, opt => opt.MapFrom(src => src.CertificateName))
                .ForMember(dest => dest.IssuingOrganization, opt => opt.MapFrom(src => src.IssuingOrganization))
                .ForMember(dest => dest.GraduationDate, opt => opt.MapFrom(src => src.GraduationDate)).ReverseMap();

        }
    }
}
