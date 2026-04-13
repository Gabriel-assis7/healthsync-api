namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IReceiveMessage
    {
        IMessageBody Body { get; }

        bool IsDelivered { get; }

        bool IsFaulted { get; }

        bool Redelivered { get; }

        TimeSpan ElapsedTime { get; }

        Uri InputAddress { get; }
    }
}
