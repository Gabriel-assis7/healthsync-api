using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication
{
    [Serializable]
    public class InvalidTokenException : BaseMessageException
    {
        public InvalidTokenException() { }

        public InvalidTokenException(string? message)
            : base(message) { }
        public InvalidTokenException(string? message, Exception? innerException)
            : base(message, innerException) { }

        public InvalidTokenException(string? tokenType, string? message)
            : base($"Invalid {tokenType} token: {message}")
        {
            TokenType = tokenType;
        }
        public InvalidTokenException(string? tokenType, string? message, Exception? innerException)
            : base($"Invalid {tokenType} token: {message}", innerException)
        {
            TokenType = tokenType;
        }
        public string? TokenType { get; private set; }
    }
}
