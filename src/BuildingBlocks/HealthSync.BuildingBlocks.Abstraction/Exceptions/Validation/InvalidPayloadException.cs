namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation
{
    [Serializable]
    public class InvalidPayloadException : ValidationException
    {
        public InvalidPayloadException() { }

        public InvalidPayloadException(string? message)
            : base(message) { }

        public InvalidPayloadException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public InvalidPayloadException(Type payloadType, string? message)
            : base(message)
        {
            PayloadType = payloadType;
        }

        public InvalidPayloadException(Type payloadType, string? message, Exception? innerException)
            : base(message, innerException)
        {
            PayloadType = payloadType;
        }

        public Type? PayloadType { get; private set; }
    }
}
