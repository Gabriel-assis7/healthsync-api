using HealthSync.BuildingBlocks.Abstraction.Exceptions.Domain;

namespace HealthSync.Services.Identity.Api.Exceptions.Verification
{
    [Serializable]
    public sealed class EmailAlreadyExistsException : EntityAlreadyExistsException

    {
        public EmailAlreadyExistsException(string email)
            : base($"Email '{email}' already exists.")
        {
            Email = email;
        }

        public string Email { get; }
    }
}
