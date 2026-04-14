namespace HealthSync.Services.Patients.Features.v1.CreatePatient.Dtos
{
    public class CreatePatientResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
