namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation
{
    [Serializable]
    public class PayloadDeserializationException : ValidationException
    {
        public PayloadDeserializationException() { }

        public PayloadDeserializationException(string? message)
            : base(message) { }

        public PayloadDeserializationException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public PayloadDeserializationException(Type targetType, string? message)
            : base(message)
        {
            TargetType = targetType;
        }

        public PayloadDeserializationException(
            Type targetType,
            string? message,
            Exception? innerException
        )
            : base(message, innerException)
        {
            TargetType = targetType;
        }

        public PayloadDeserializationException(string? format, Type targetType, string? message)
            : base(message)
        {
            Format = format;
            TargetType = targetType;
        }

        public Type? TargetType { get; private set; }

        public string? Format { get; private set; }
    }
}
