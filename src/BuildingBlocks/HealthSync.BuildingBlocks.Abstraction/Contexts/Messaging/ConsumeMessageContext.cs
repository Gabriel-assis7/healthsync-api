namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Messaging
{
    public interface IConsumeMessageContext<out T>
        where T : class
    {
        T Message { get; }

        Task NotifyConsumedMessage();

        Task NotifyFaultedMessage(Exception exception);
    }
}
