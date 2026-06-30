namespace ClinicManagement.API.Domain.Entities
{
    public class DoctorCertificate
    {
        public Guid Id { get; set; }
        public string CertificateName { get; set; } 
        public string IssuingOrganization { get; set; } 
        public DateTime GraduationDate { get; set; } 

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
