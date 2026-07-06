using System;
using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Identity.Exceptions.Tokens
{
    /// <summary>
    /// Thrown when a refresh token is malformed, tampered with, or fails validation.
    /// Refresh tokens are used to obtain new access tokens without re-authenticating.
    /// Inherits from InvalidTokenException to integrate with generic token validation infrastructure.
    /// </summary>
    [Serializable]
    public sealed class InvalidRefreshTokenException : InvalidTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class.
        /// </summary>
        public InvalidRefreshTokenException()
            : base("RefreshToken", "The refresh token is invalid.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class with a message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InvalidRefreshTokenException(string? message)
            : base("RefreshToken", message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class with a message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public InvalidRefreshTokenException(string? message, Exception? innerException)
            : base("RefreshToken", message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class with a reason for invalidity.
        /// </summary>
        /// <param name="reason">The reason the token is invalid (e.g., "MalformedToken", "InvalidSignature", "UserIdMismatch").</param>
        /// <param name="message">Additional context about the invalidity.</param>
        public InvalidRefreshTokenException(string? reason, string? message)
            : base("RefreshToken", $"Invalid refresh token. Reason: {reason}. {message}")
        {
            Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class with a reason and inner exception.
        /// </summary>
        /// <param name="reason">The reason the token is invalid.</param>
        /// <param name="message">Additional context about the invalidity.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public InvalidRefreshTokenException(string? reason, string? message, Exception? innerException)
            : base("RefreshToken", $"Invalid refresh token. Reason: {reason}. {message}", innerException)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets the reason this refresh token was deemed invalid (e.g., "MalformedToken", "InvalidSignature", "UserIdMismatch").
        /// </summary>
        public string? Reason { get; private set; }
    }
}
