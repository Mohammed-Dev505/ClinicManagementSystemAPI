namespace ClinicManagement.API.Application.DTOs
{
    public class ReadDoctorCertificateDto
    {
        public Guid Id { get; set; }
        public string CertificateName { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime GraduationDate { get; set; }
        public Guid DoctorId { get; set; } 
    }
}
