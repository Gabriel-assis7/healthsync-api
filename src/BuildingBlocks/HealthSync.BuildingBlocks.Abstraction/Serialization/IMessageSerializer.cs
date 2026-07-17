using HealthSync.BuildingBlocks.Abstraction.Contexts.Message;

namespace HealthSync.BuildingBlocks.Abstraction.Serialization
{
    /// <summary>
    /// Serializes messages into message bodies using a consistent wire contract.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// The default content type emitted when no contextual override is supplied.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Serializes the supplied message into a message body.
        /// A null message is rejected with <see cref="ArgumentNullException"/> and is not encoded as an empty body.
        /// The effective content type is taken from <paramref name="context"/> when present; otherwise this serializer's <see cref="ContentType"/> is used.
        /// </summary>
        IMessageBody Serialize(object? message, IMessageSerializationContext? context = null);

        /// <summary>
        /// Deserializes a message body into an object of the requested message type.
        /// </summary>
        object? Deserialize(Type messageType, IMessageBody body, IMessageDeserializationContext? context = null);

        /// <summary>
        /// Deserializes a message body into the requested generic message type.
        /// </summary>
        T? Deserialize<T>(IMessageBody body, IMessageDeserializationContext? context = null);
    }
}
