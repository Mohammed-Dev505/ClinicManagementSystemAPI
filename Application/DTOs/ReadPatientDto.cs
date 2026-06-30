namespace ClinicManagement.API.Application.DTOs
{
    public class ReadPatientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BloodType {  get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ChronicDiseases { get; set; } = string.Empty;
        public int Age => DateTime.Today.Year - DateOfBirth.Year - (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    }
}
