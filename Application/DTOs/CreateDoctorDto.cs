namespace ClinicManagement.API.Application.DTOs
{
    public class CreateDoctorDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Medicallicense {  get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;

        public Guid SpecialtyId { get; set; }

        List<CreateDoctorCertificateDto> doctorCertificateCreateDtos = new List<CreateDoctorCertificateDto>();
    }
}
