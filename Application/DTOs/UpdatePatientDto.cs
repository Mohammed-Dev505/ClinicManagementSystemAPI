namespace ClinicManagement.API.Application.DTOs
{
    public class UpdatePatientDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BloodType {  get; set; } = string.Empty;
        public string ChronicDiseases { get; set; } = string.Empty;
    }
}
