namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IMessageEnvelopeContext
    {
        IMessageContext MessageContext { get; }

        IReceiveMessage ReceiveMessage { get; }

        IMessageBody Body { get; }
    }
}
