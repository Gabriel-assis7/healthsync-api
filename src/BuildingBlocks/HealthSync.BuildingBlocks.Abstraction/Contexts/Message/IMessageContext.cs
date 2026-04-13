namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IMessageContext
    {
        Guid? MessageId { get; }

        Guid? CorrelationId { get; }

        Guid? RequestId { get; }

        Guid? ConversationId { get; }

        Guid? InitiatorId { get; }

        Uri? SourceAddress { get; }

        Uri? DestinationAddress { get; }

        Uri? ResponseAddress { get; }

        Uri? FaultAddress { get; }

        DateTime? ExpirationTime { get; }

        DateTime? SentTime { get; }

        IHeaders Headers { get; }
    }
}
