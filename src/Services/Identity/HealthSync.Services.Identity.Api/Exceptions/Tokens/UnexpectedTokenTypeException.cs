using System;
using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;

namespace HealthSync.Services.Identity.Identity.Exceptions.Tokens
{
    /// <summary>
    /// Thrown when a token is valid but is of the wrong type for the operation being attempted.
    /// For example, using a refresh token where an access token is required.
    /// Inherits from InvalidTokenException to integrate with token validation infrastructure.
    /// </summary>
    [Serializable]
    public sealed class UnexpectedTokenTypeException : InvalidTokenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenTypeException"/> class.
        /// </summary>
        public UnexpectedTokenTypeException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenTypeException"/> class with a message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public UnexpectedTokenTypeException(string? message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenTypeException"/> class with a message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public UnexpectedTokenTypeException(string? message, Exception? innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenTypeException"/> class with expected and actual token types.
        /// </summary>
        /// <param name="expectedTokenType">The type of token that was expected (e.g., "AccessToken").</param>
        /// <param name="actualTokenType">The type of token that was provided (e.g., "RefreshToken").</param>
        public UnexpectedTokenTypeException(string? expectedTokenType, string? actualTokenType)
            : base($"Expected token type '{expectedTokenType}' but received '{actualTokenType}'.")
        {
            ExpectedTokenType = expectedTokenType;
            ActualTokenType = actualTokenType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedTokenTypeException"/> class with token types and inner exception.
        /// </summary>
        /// <param name="expectedTokenType">The type of token that was expected.</param>
        /// <param name="actualTokenType">The type of token that was provided.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public UnexpectedTokenTypeException(string? expectedTokenType, string? actualTokenType, Exception? innerException)
            : base($"Expected token type '{expectedTokenType}' but received '{actualTokenType}'.", innerException)
        {
            ExpectedTokenType = expectedTokenType;
            ActualTokenType = actualTokenType;
        }

        /// <summary>
        /// Gets the token type that was expected for the operation.
        /// </summary>
        public string? ExpectedTokenType { get; private set; }

        /// <summary>
        /// Gets the token type that was actually provided.
        /// </summary>
        public string? ActualTokenType { get; private set; }
    }
}
