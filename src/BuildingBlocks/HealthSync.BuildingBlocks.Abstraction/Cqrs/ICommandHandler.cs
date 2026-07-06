using HealthSync.Services.Identity.Identity.Core.Cqrs;

namespace HealthSync.BuildingBlocks.Abstraction.Cqrs
{
    public interface ICommandHandler<in TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
