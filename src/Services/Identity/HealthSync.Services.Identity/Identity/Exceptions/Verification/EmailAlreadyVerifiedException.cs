using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Identity.Exceptions.Verification
{
    [Serializable]
    public sealed class EmailAlreadyVerifiedException : EntityAlreadyVerifiedException

    {
        public EmailAlreadyVerifiedException(string email, string message)
            : base($"Email '{email}' is already verified. {message}")
        {
            Email = email;
        }

        public string Email { get; }
    }
}
