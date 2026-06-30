namespace ClinicManagement.API.Application.DTOs
{
    public class UpdateDoctorDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MedicalLicense {  get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public Guid SpecialtyId { get; set; }
    }
}
