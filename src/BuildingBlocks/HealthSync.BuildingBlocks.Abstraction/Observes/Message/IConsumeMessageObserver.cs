using HealthSync.BuildingBlocks.Abstraction.Contexts.Message;

namespace HealthSync.BuildingBlocks.Abstraction.Observes.Message
{
    public interface IConsumeMessageMessageObserver
    {
        Task PreConsumeMessage<T>(IConsumeMessageObserverContext<T> context)
            where T : class;

        Task PostConsumeMessage<T>(IConsumeMessageObserverContext<T> context)
            where T : class;

        Task ConsumeMessageFault<T>(IConsumeMessageObserverContext<T> context, Exception exception)
            where T : class;
    }
}
