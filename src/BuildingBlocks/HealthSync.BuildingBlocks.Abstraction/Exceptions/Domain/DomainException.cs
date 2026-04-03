namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Domain
{
    [Serializable]
    public class DomainException : BaseMessageException
    {
        public DomainException() { }

        public DomainException(string? message)
            : base(message) { }

        public DomainException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public DomainException(string? domain, string? message)
            : base($"Domain '{domain}' error: {message}")
        {
            Domain = domain;
        }

        public DomainException(string? domain, string? message, Exception? innerException)
            : base($"Domain '{domain}' error: {message}", innerException)
        {
            Domain = domain;
        }

        public string? Domain { get; private set; }
    }
}
