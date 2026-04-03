namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation
{
    [Serializable]
    public class ValidationException : BaseMessageException
    {
        public ValidationException() { }

        public ValidationException(string? message)
            : base(message) { }

        public ValidationException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public ValidationException(string? fieldName, string? message)
            : base($"Validation failed for field '{fieldName}': {message}")
        {
            FieldName = fieldName;
        }

        public ValidationException(string? fieldName, string? message, Exception? innerException)
            : base($"Validation failed for field '{fieldName}': {message}", innerException)
        {
            FieldName = fieldName;
        }

        public string? FieldName { get; private set; }
    }
}
