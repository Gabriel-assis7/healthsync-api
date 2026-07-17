namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IMessageSerializationContext
    {
        Type? MessageType { get; }

        string? ContentType { get; }

        IHeaders? Headers { get; }
    }
}
