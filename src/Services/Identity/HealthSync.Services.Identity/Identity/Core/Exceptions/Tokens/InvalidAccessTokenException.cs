using System;
using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Identity.Core.Exceptions.Tokens
{
    /// <summary>
    /// Thrown when an access token is malformed, tampered with, or fails cryptographic validation.
    /// This is distinct from ExpiredTokenException (TTL) and RevokedTokenException (explicitly revoked).
    /// Inherits from InvalidTokenException to integrate with generic token validation infrastructure.
    /// </summary>
    [Serializable]
    public sealed class InvalidAccessTokenException : InvalidTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccessTokenException"/> class.
        /// </summary>
        public InvalidAccessTokenException()
            : base("AccessToken", "The access token is invalid.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccessTokenException"/> class with a message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InvalidAccessTokenException(string? message)
            : base("AccessToken", message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccessTokenException"/> class with a message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public InvalidAccessTokenException(string? message, Exception? innerException)
            : base("AccessToken", message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccessTokenException"/> class with a reason for invalidity.
        /// </summary>
        /// <param name="reason">The reason the token is invalid (e.g., "MalformedToken", "InvalidSignature", "ClaimsMissing").</param>
        /// <param name="message">Additional context about the invalidity.</param>
        public InvalidAccessTokenException(string? reason, string? message)
            : base("AccessToken", $"Invalid access token. Reason: {reason}. {message}")
        {
            Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccessTokenException"/> class with a reason and inner exception.
        /// </summary>
        /// <param name="reason">The reason the token is invalid.</param>
        /// <param name="message">Additional context about the invalidity.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public InvalidAccessTokenException(string? reason, string? message, Exception? innerException)
            : base("AccessToken", $"Invalid access token. Reason: {reason}. {message}", innerException)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets the reason this access token was deemed invalid (e.g., "MalformedToken", "InvalidSignature", "ClaimsMissing").
        /// </summary>
        public string? Reason { get; private set; }
    }
}
