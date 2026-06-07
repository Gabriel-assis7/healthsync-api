namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation
{
    [Serializable]
    public class InvalidCredentialsException : ValidationException
    {
        public InvalidCredentialsException() { }

        public InvalidCredentialsException(string? message)
            : base(message) { }

        public InvalidCredentialsException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public InvalidCredentialsException(Type payloadType, string? message)
            : base(message)
        {
            PayloadType = payloadType;
        }

        public InvalidCredentialsException(Type payloadType, string? message, Exception? innerException)
            : base(message, innerException)
        {
            PayloadType = payloadType;
        }

        public Type? PayloadType { get; private set; }
    }
}
