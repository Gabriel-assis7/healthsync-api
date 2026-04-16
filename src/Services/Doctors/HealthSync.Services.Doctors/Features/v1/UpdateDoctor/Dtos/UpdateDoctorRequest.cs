namespace HealthSync.Services.Doctors.Features.v1.UpdateDoctor.Dtos
{
    public class UpdateDoctorRequest
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Specialty { get; set; }
        public string? LicenseNumber { get; set; }
        public string? ClinicAddress { get; set; }
        public string? Availability { get; set; }
    }
}
