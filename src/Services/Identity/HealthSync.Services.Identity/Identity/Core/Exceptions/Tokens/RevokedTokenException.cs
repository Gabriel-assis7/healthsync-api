using System;
using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Identity.Core.Exceptions.Tokens
{
    /// <summary>
    /// Thrown when a token has been explicitly revoked and is no longer valid.
    /// This occurs when a user logs out, changes password, or an administrator revokes access.
    /// Inherits from InvalidTokenException to integrate with token validation infrastructure.
    /// </summary>
    [Serializable]
    public sealed class RevokedTokenException : InvalidTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RevokedTokenException"/> class.
        /// </summary>
        public RevokedTokenException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokedTokenException"/> class with a message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public RevokedTokenException(string? message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokedTokenException"/> class with a message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public RevokedTokenException(string? message, Exception? innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokedTokenException"/> class with token type context.
        /// </summary>
        /// <param name="tokenType">The type of token that was revoked (e.g., "AccessToken", "RefreshToken").</param>
        /// <param name="message">The error message.</param>
        public RevokedTokenException(string? tokenType, string? message)
            : base(tokenType, $"Token has been revoked: {message}")
        {
            RevokedAt = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokedTokenException"/> class with token type and revocation time.
        /// </summary>
        /// <param name="tokenType">The type of token that was revoked.</param>
        /// <param name="revokedAt">The UTC time when the token was revoked.</param>
        /// <param name="reason">The reason for revocation (e.g., "UserLoggedOut", "PasswordChanged").</param>
        public RevokedTokenException(string? tokenType, DateTimeOffset revokedAt, string? reason = null)
            : base(tokenType, $"Token revoked at {revokedAt:O}. Reason: {reason}")
        {
            RevokedAt = revokedAt;
            Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokedTokenException"/> class with token type, revocation time, and inner exception.
        /// </summary>
        /// <param name="tokenType">The type of token that was revoked.</param>
        /// <param name="revokedAt">The UTC time when the token was revoked.</param>
        /// <param name="reason">The reason for revocation.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public RevokedTokenException(string? tokenType, DateTimeOffset revokedAt, string? reason, Exception? innerException)
            : base(tokenType, $"Token revoked at {revokedAt:O}. Reason: {reason}", innerException)
        {
            RevokedAt = revokedAt;
            Reason = reason;
        }

        /// <summary>
        /// Gets the UTC time when the token was revoked, if available.
        /// </summary>
        public DateTimeOffset? RevokedAt { get; private set; }

        /// <summary>
        /// Gets the reason for revocation (e.g., "UserLoggedOut", "PasswordChanged", "AdminRevoked").
        /// </summary>
        public string? Reason { get; private set; }
    }
}
