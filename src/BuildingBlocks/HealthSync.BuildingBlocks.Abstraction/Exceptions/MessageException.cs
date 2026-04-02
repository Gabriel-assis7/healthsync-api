using System;

namespace HealthSync.BuildingBlocks.Abstraction.Exceptions
{
    [Serializable]
    public class MessageException : BaseMessageException
    {
        public MessageException() { }

        public MessageException(Type messageType, string message)
            : base(message)
        {
            MessageType = messageType;
        }

        public MessageException(Type messageType, string message, Exception innerException)
            : base(message, innerException)
        {
            MessageType = messageType;
        }

        public Type? MessageType { get; private set; }
    }
}
