namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IMessageDeserializationContext
    {
        Type? MessageType { get; }

        string? ContentType { get; }

        IHeaders? Headers { get; }
    }
}
