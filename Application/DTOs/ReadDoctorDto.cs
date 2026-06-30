namespace ClinicManagement.API.Application.DTOs
{
    public class ReadDoctorDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MedicalLicense { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;
    }
}
