using System.Data;
using HealthSync.BuildingBlocks.Abstraction.Events;
using HealthSync.BuildingBlocks.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MediatR;

namespace HealthSync.BuildingBlocks.Core.Persistence.EfCore;

public class TxDbBehavior<TRequest, TResponse>(
    ILogger<TxDbBehavior<TRequest, TResponse>> logger,
    IDomainEventPublisher domainEventPublisher,
    IDomainEventsAccessor domainEventsAccessor,
    IDbFacadeResolver dbFacadeResolver
    )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest req,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (req is not ITxRequest)
        {
            return await next();
        }

        var strategy = dbFacadeResolver.Database.CreateExecutionStrategy();

        var res = await strategy.ExecuteAsync(async () =>
        {
            var currentTransaction = dbFacadeResolver.Database.CurrentTransaction is not null;
            var transaction = dbFacadeResolver.Database.CurrentTransaction ?? await dbFacadeResolver.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            try
            {
                var response = await next();

                logger.LogInformation(
                    "{Prefix} Open the transaction for {MediatrRequest}",
                    nameof(TxDbBehavior<TRequest, TResponse>),
                    typeof(TRequest).FullName
                );

                var domainEvents = domainEventsAccessor.DequeueUncommittedDomainEvents();
                await domainEventPublisher.PublishAsync(domainEvents, cancellationToken);

                if (currentTransaction == false)
                {
                    await transaction.CommitAsync(cancellationToken);
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling transaction for request {RequestName}", typeof(TRequest).Name);

                if (currentTransaction == false)
                {
                    await transaction.RollbackAsync(cancellationToken);
                }

                throw;
            }
        });

        return res;
    }
}