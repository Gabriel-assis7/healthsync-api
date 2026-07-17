using HealthSync.BuildingBlocks.Abstraction.Contexts.Message;

namespace HealthSync.BuildingBlocks.Abstraction.Serialization
{
    /// <summary>
    /// Deserializes values or message bodies into objects using a single authoritative target type.
    /// </summary>
    public interface IObjectDeserializer
    {
        /// <summary>
        /// Deserializes the supplied value into an object.
        /// The effective target type is resolved by precedence from <paramref name="messageType"/> first, then <paramref name="context"/>.MessageType.
        /// If both are supplied and their values differ, implementations must reject the call with <see cref="ArgumentException"/>.
        /// If they agree, or only one is supplied, the non-null value is used.
        /// </summary>
        object? Deserialize(object? value, Type? messageType = null, IMessageDeserializationContext? context = null);

        /// <summary>
        /// Deserializes the supplied message body into an object using the same target-type precedence and conflict rules as the value overload.
        /// </summary>
        object? Deserialize(IMessageBody body, Type? messageType = null, IMessageDeserializationContext? context = null);
    }
}
