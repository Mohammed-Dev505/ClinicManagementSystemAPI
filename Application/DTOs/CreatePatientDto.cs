namespace ClinicManagement.API.Application.DTOs
{
    public class CreatePatientDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BloodType {  get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ChronicDiseases { get; set; } = string.Empty;
    }
}
