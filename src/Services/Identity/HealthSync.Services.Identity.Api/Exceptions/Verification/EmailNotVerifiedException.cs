using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Api.Exceptions.Verification
{
    [Serializable]
    public sealed class EmailNotVerifiedException : EntityAlreadyVerifiedException

    {
        public EmailNotVerifiedException(string email)
            : base($"Email '{email}' has not been verified.")
        {
            Email = email;
        }

        public string Email { get; }
    }
}
