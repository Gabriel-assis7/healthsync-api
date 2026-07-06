using HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation;

namespace HealthSync.Services.Identity.Api.Exceptions.Verification
{
    [Serializable]
    public sealed class EmailNotFoundException : InvalidCredentialsException

    {
        public EmailNotFoundException(string email)
            : base($"Email '{email}' was not found.")
        {
            Email = email;
        }

        public string Email { get; }
    }
}
