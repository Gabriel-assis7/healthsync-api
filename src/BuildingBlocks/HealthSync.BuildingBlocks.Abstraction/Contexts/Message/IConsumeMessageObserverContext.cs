namespace HealthSync.BuildingBlocks.Abstraction.Contexts.Message
{
    public interface IConsumeMessageObserverContext<out T>
        where T : class
    {
        T Message { get; }

        Task NotifyConsumedMessage(TimeSpan timeSpan, string consumerType);

        Task NotifyFaultedMessage(TimeSpan timeSpan, string consumerType, Exception exception);
    }
}
