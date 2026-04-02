using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions
{
    [Serializable]
    public class BaseMessageException : Exception
    {
        public BaseMessageException() { }

        public BaseMessageException(string? message)
            : base(message) { }

        public BaseMessageException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
