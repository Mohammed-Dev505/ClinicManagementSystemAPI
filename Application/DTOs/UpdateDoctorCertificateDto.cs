namespace ClinicManagement.API.Application.DTOs
{
    public class UpdateDoctorCertificateDto
    {
        public string? CertificateName { get; set; }
        public string? IssuingOrganization { get; set; }
        public DateTime? GraduationDate { get; set; }
    }
}
