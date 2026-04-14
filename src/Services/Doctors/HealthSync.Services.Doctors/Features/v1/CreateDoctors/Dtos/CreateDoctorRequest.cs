namespace HealthSync.Services.Doctors.Features.v1.CreateDoctors.Dtos
{
    public class CreateDoctorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialty { get; set; }
        public string LicenseNumber { get; set; }
        public string ClinicAddress { get; set; }
        public string Availability { get; set; }
    }
}
