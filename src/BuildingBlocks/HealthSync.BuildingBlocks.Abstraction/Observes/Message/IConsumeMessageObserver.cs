using HealthSync.BuildingBlocks.Abstraction.Contexts.Messaging;

namespace HealthSync.BuildingBlocks.Abstraction.Observes.Message
{
    public interface IConsumeMessageMessageObserver
    {
        Task PreConsumeMessage<T>(IConsumeMessageContext<T> context)
            where T : class;

        Task PostConsumeMessage<T>(IConsumeMessageContext<T> context)
            where T : class;

        Task ConsumeMessageFault<T>(IConsumeMessageContext<T> context, Exception exception)
            where T : class;
    }
}
