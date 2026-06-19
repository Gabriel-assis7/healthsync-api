using System;
using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Identity.Exceptions.Tokens
{
    [Serializable]
    public sealed class ExpiredTokenException : InvalidTokenException
    {
        public ExpiredTokenException() { }

        public ExpiredTokenException(string? message)
            : base(message) { }

        public ExpiredTokenException(string? message, Exception? innerException)
            : base(message, innerException) { }
        public ExpiredTokenException(string? tokenType, string? message)
            : base(tokenType, $"Token has expired: {message}")
        {
            ExpiresAt = null;
        }

        public ExpiredTokenException(string? tokenType, DateTimeOffset expiresAt, string? message = null)
            : base(tokenType, $"Token expired at {expiresAt:O}. {message}")
        {
            ExpiresAt = expiresAt;
        }

        public ExpiredTokenException(string? tokenType, DateTimeOffset expiresAt, string? message, Exception? innerException)
            : base(tokenType, $"Token expired at {expiresAt:O}. {message}", innerException)
        {
            ExpiresAt = expiresAt;
        }

        public DateTimeOffset? ExpiresAt { get; private set; }
    }
}
